#include "stdafx.h"

typedef struct tagPerformOleDragDropArgs
{
    HWND hWnd;
    LPCWSTR lpFileName;
} PerformOleDragDropArgs;

extern "C" HRESULT GetUIObjectOfFile(HWND hwnd, LPCWSTR pszPath, REFIID riid, void** ppv)
{
    *ppv = nullptr;
    HRESULT hr;
    LPITEMIDLIST pidl;
    SFGAOF sfgao;
    if (SUCCEEDED(hr = SHParseDisplayName(pszPath, NULL, &pidl, 0, &sfgao)))
    {
        IShellFolder* psf;
        LPCITEMIDLIST pidlChild;
        if (SUCCEEDED(hr = SHBindToParent(pidl, IID_IShellFolder,
            reinterpret_cast<void**>(&psf), &pidlChild)))
        {
            hr = psf->GetUIObjectOf(hwnd, 1, &pidlChild, riid, nullptr, ppv);
            psf->Release();
        }
        CoTaskMemFree(pidl);
    }
    return hr;
}

extern "C" __declspec(dllexport) HRESULT WINAPI PerformOleDragDrop(PerformOleDragDropArgs* lpParameter)
{
    const auto hWnd = lpParameter->hWnd;
    const auto lpFileName = lpParameter->lpFileName;

    CoInitialize(nullptr);

    const auto pUnk = static_cast<IUnknown*>(GetProp(hWnd, TEXT("OleDropTargetInterface")));
    if (!pUnk) return E_FAIL;

    IDropTarget* pDropTarget;
    auto hr = pUnk->QueryInterface(IID_IDropTarget, reinterpret_cast<void**>(&pDropTarget));

    if (SUCCEEDED(hr))
    {
        IDataObject* pDataObject;

        hr = GetUIObjectOfFile(nullptr, lpFileName, IID_IDataObject, reinterpret_cast<void**>(&pDataObject));

        if (SUCCEEDED(hr))
        {
            DWORD dwEffect = DROPEFFECT_COPY;

            hr = pDropTarget->DragEnter(pDataObject, MK_LBUTTON, {0, 0}, &dwEffect);

            if (SUCCEEDED(hr))
            {
                if ((dwEffect & DROPEFFECT_COPY) == DROPEFFECT_COPY)
                    pDropTarget->Drop(pDataObject, MK_LBUTTON, {0, 0}, &dwEffect);
                else
                    pDropTarget->DragLeave();
            }

            pDataObject->Release();
        }

        pDropTarget->Release();
    }

    CoUninitialize();

    return hr;
}
