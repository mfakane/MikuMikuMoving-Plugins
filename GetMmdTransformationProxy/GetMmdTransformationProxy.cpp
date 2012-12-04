// GetMmdTransformationProxy.cpp : DLL アプリケーション用にエクスポートされる関数を定義します。
//

#include "stdafx.h"

#define DEFINE_PROC(hModule, name) auto p##name = reinterpret_cast<decltype(name)*>(GetProcAddress(hModule, #name))

extern "C" int _fltused = 0x9875;

extern "C" __declspec(dllexport) DWORD WINAPI RemoteEntryPoint(LPVOID lpParameter)
{
	auto mmdModule = GetModuleHandle("MikuMikuDance.exe");

	DEFINE_PROC(mmdModule, ExpGetFrameTime);
	DEFINE_PROC(mmdModule, ExpGetPmdNum);
	DEFINE_PROC(mmdModule, ExpGetPmdFilename);
	DEFINE_PROC(mmdModule, ExpGetPmdOrder);
	DEFINE_PROC(mmdModule, ExpGetPmdMatNum);
	DEFINE_PROC(mmdModule, ExpGetPmdMaterial);
	DEFINE_PROC(mmdModule, ExpGetPmdBoneNum);
	DEFINE_PROC(mmdModule, ExpGetPmdBoneName);
	DEFINE_PROC(mmdModule, ExpGetPmdBoneWorldMat);
	DEFINE_PROC(mmdModule, ExpGetPmdMorphNum);
	DEFINE_PROC(mmdModule, ExpGetPmdMorphName);
	DEFINE_PROC(mmdModule, ExpGetPmdMorphValue);
	DEFINE_PROC(mmdModule, ExpGetPmdDisp);
	DEFINE_PROC(mmdModule, ExpGetPmdID);

	DEFINE_PROC(mmdModule, ExpGetAcsNum);
	DEFINE_PROC(mmdModule, ExpGetPreAcsNum);
	DEFINE_PROC(mmdModule, ExpGetAcsFilename);
	DEFINE_PROC(mmdModule, ExpGetAcsOrder);
	DEFINE_PROC(mmdModule, ExpGetAcsWorldMat);
	DEFINE_PROC(mmdModule, ExpGetAcsX);
	DEFINE_PROC(mmdModule, ExpGetAcsY);
	DEFINE_PROC(mmdModule, ExpGetAcsZ);
	DEFINE_PROC(mmdModule, ExpGetAcsRx);
	DEFINE_PROC(mmdModule, ExpGetAcsRy);
	DEFINE_PROC(mmdModule, ExpGetAcsRz);
	DEFINE_PROC(mmdModule, ExpGetAcsSi);
	DEFINE_PROC(mmdModule, ExpGetAcsTr);
	DEFINE_PROC(mmdModule, ExpGetAcsDisp);
	DEFINE_PROC(mmdModule, ExpGetAcsID);
	DEFINE_PROC(mmdModule, ExpGetAcsMatNum);
	DEFINE_PROC(mmdModule, ExpGetAcsMaterial);

	DEFINE_PROC(mmdModule, ExpGetCurrentObject);
	DEFINE_PROC(mmdModule, ExpGetCurrentMaterial);
	DEFINE_PROC(mmdModule, ExpGetCurrentTechnic);
	DEFINE_PROC(mmdModule, ExpSetRenderRepeatCount);
	DEFINE_PROC(mmdModule, ExpGetRenderRepeatCount);

	char pipeName[256];

	RtlZeroMemory(pipeName, sizeof(pipeName));
	wsprintf(pipeName, "\\\\.\\pipe\\LS_MMM_GMTP_MI_%d", (unsigned int)lpParameter);

	auto pipe = CreateFile
	(
		pipeName,
		GENERIC_READ | GENERIC_WRITE,
		0,
		nullptr,
		OPEN_EXISTING,
		FILE_ATTRIBUTE_NORMAL,
		nullptr
	);

	if (pipe != INVALID_HANDLE_VALUE)
	{
		BYTE command;
		DWORD bytesReadOrWritten;

		while (ReadFile(pipe, &command, 1, &bytesReadOrWritten, nullptr))
		{
			BYTE length = 0;
			char name[256];
			BYTE argLength = 0;
			DWORD args[256];

			RtlZeroMemory(name, sizeof(name));
			RtlZeroMemory(args, sizeof(args));

			if (bytesReadOrWritten == 0 ||
				command == 0)
				break;

			ReadFile(pipe, &length, 1, &bytesReadOrWritten, nullptr);
			ReadFile(pipe, name, length, &bytesReadOrWritten, nullptr);

			ReadFile(pipe, &argLength, 1, &bytesReadOrWritten, nullptr);

			for (int i = 0; i < argLength; i++)
				ReadFile(pipe, &args[i], sizeof(args[i]), &bytesReadOrWritten, nullptr);

			if (lstrcmp(name, "ExpGetFrameTime") == 0)
			{
				auto buffer = pExpGetFrameTime();

				WriteFile(pipe, &buffer, sizeof(buffer), &bytesReadOrWritten, nullptr);
			}
			else if (lstrcmp(name, "ExpGetPmdNum") == 0)
			{
				auto buffer = pExpGetPmdNum();

				WriteFile(pipe, &buffer, sizeof(buffer), &bytesReadOrWritten, nullptr);
			}
			else if (lstrcmp(name, "ExpGetPmdFilename") == 0)
			{
				auto str = pExpGetPmdFilename(args[0]);

				if (str == nullptr)
				{
					BYTE strLength = 0;

					WriteFile(pipe, &strLength, 1, &bytesReadOrWritten, nullptr);
				}
				else
				{
					auto strLength = (BYTE)lstrlen(str);

					WriteFile(pipe, &strLength, 1, &bytesReadOrWritten, nullptr);
					WriteFile(pipe, str, strLength, &bytesReadOrWritten, nullptr);
				}
			}
			else if (lstrcmp(name, "ExpGetPmdOrder") == 0)
			{
				auto buffer = pExpGetPmdOrder(args[0]);

				WriteFile(pipe, &buffer, sizeof(buffer), &bytesReadOrWritten, nullptr);
			}
			else if (lstrcmp(name, "ExpGetPmdMatNum") == 0)
			{
				auto buffer = pExpGetPmdMatNum(args[0]);

				WriteFile(pipe, &buffer, sizeof(buffer), &bytesReadOrWritten, nullptr);
			}
			else if (lstrcmp(name, "ExpGetPmdBoneNum") == 0)
			{
				auto buffer = pExpGetPmdBoneNum(args[0]);

				WriteFile(pipe, &buffer, sizeof(buffer), &bytesReadOrWritten, nullptr);
			}
			else if (lstrcmp(name, "ExpGetPmdBoneName") == 0)
			{
				auto str = pExpGetPmdBoneName(args[0], args[1]);

				if (str == nullptr)
				{
					BYTE strLength = 0;

					WriteFile(pipe, &strLength, 1, &bytesReadOrWritten, nullptr);
				}
				else
				{
					auto strLength = (BYTE)lstrlen(str);

					WriteFile(pipe, &strLength, 1, &bytesReadOrWritten, nullptr);
					WriteFile(pipe, str, strLength, &bytesReadOrWritten, nullptr);
				}
			}
			else if (lstrcmp(name, "ExpGetPmdBoneWorldMat") == 0)
			{
				auto buffer = pExpGetPmdBoneWorldMat(args[0], args[1]);

				WriteFile(pipe, &buffer, sizeof(buffer), &bytesReadOrWritten, nullptr);
			}
			else if (lstrcmp(name, "ExpGetPmdMorphNum") == 0)
			{
				auto buffer = pExpGetPmdMorphNum(args[0]);

				WriteFile(pipe, &buffer, sizeof(buffer), &bytesReadOrWritten, nullptr);
			}
			else if (lstrcmp(name, "ExpGetPmdMorphName") == 0)
			{
				auto str = pExpGetPmdMorphName(args[0], args[1]);

				if (str == nullptr)
				{
					BYTE strLength = 0;

					WriteFile(pipe, &strLength, 1, &bytesReadOrWritten, nullptr);
				}
				else
				{
					auto strLength = (BYTE)lstrlen(str);

					WriteFile(pipe, &strLength, 1, &bytesReadOrWritten, nullptr);
					WriteFile(pipe, str, strLength, &bytesReadOrWritten, nullptr);
				}
			}
			else if (lstrcmp(name, "ExpGetPmdMorphValue") == 0)
			{
				auto buffer = pExpGetPmdMorphValue(args[0], args[1]);

				WriteFile(pipe, &buffer, sizeof(buffer), &bytesReadOrWritten, nullptr);
			}
			else if (lstrcmp(name, "ExpGetPmdDisp") == 0)
			{
				auto buffer = pExpGetPmdDisp(args[0]);

				WriteFile(pipe, &buffer, sizeof(buffer), &bytesReadOrWritten, nullptr);
			}
			else if (lstrcmp(name, "ExpGetPmdID") == 0)
			{
				auto buffer = pExpGetPmdID(args[0]);

				WriteFile(pipe, &buffer, sizeof(buffer), &bytesReadOrWritten, nullptr);
			}

			else if (lstrcmp(name, "ExpGetAcsNum") == 0)
			{
				auto buffer = pExpGetAcsNum();

				WriteFile(pipe, &buffer, sizeof(buffer), &bytesReadOrWritten, nullptr);
			}
			else if (lstrcmp(name, "ExpGetPreAcsNum") == 0)
			{
				auto buffer = pExpGetPreAcsNum();

				WriteFile(pipe, &buffer, sizeof(buffer), &bytesReadOrWritten, nullptr);
			}
			else if (lstrcmp(name, "ExpGetAcsFilename") == 0)
			{
				auto str = pExpGetAcsFilename(args[0]);

				if (str == nullptr)
				{
					BYTE strLength = 0;

					WriteFile(pipe, &strLength, 1, &bytesReadOrWritten, nullptr);
				}
				else
				{
					auto strLength = (BYTE)lstrlen(str);

					WriteFile(pipe, &strLength, 1, &bytesReadOrWritten, nullptr);
					WriteFile(pipe, str, strLength, &bytesReadOrWritten, nullptr);
				}
			}
			else if (lstrcmp(name, "ExpGetAcsOrder") == 0)
			{
				auto buffer = pExpGetAcsOrder(args[0]);

				WriteFile(pipe, &buffer, sizeof(buffer), &bytesReadOrWritten, nullptr);
			}
			else if (lstrcmp(name, "ExpGetAcsX") == 0)
			{
				auto buffer = pExpGetAcsX(args[0]);

				WriteFile(pipe, &buffer, sizeof(buffer), &bytesReadOrWritten, nullptr);
			}
			else if (lstrcmp(name, "ExpGetAcsY") == 0)
			{
				auto buffer = pExpGetAcsY(args[0]);

				WriteFile(pipe, &buffer, sizeof(buffer), &bytesReadOrWritten, nullptr);
			}
			else if (lstrcmp(name, "ExpGetAcsZ") == 0)
			{
				auto buffer = pExpGetAcsZ(args[0]);

				WriteFile(pipe, &buffer, sizeof(buffer), &bytesReadOrWritten, nullptr);
			}
			else if (lstrcmp(name, "ExpGetAcsRx") == 0)
			{
				auto buffer = pExpGetAcsRx(args[0]);

				WriteFile(pipe, &buffer, sizeof(buffer), &bytesReadOrWritten, nullptr);
			}
			else if (lstrcmp(name, "ExpGetAcsRy") == 0)
			{
				auto buffer = pExpGetAcsRy(args[0]);

				WriteFile(pipe, &buffer, sizeof(buffer), &bytesReadOrWritten, nullptr);
			}
			else if (lstrcmp(name, "ExpGetAcsRz") == 0)
			{
				auto buffer = pExpGetAcsRz(args[0]);

				WriteFile(pipe, &buffer, sizeof(buffer), &bytesReadOrWritten, nullptr);
			}
			else if (lstrcmp(name, "ExpGetAcsSi") == 0)
			{
				auto buffer = pExpGetAcsSi(args[0]);

				WriteFile(pipe, &buffer, sizeof(buffer), &bytesReadOrWritten, nullptr);
			}
			else if (lstrcmp(name, "ExpGetAcsTr") == 0)
			{
				auto buffer = pExpGetAcsTr(args[0]);

				WriteFile(pipe, &buffer, sizeof(buffer), &bytesReadOrWritten, nullptr);
			}
			else if (lstrcmp(name, "ExpGetAcsDisp") == 0)
			{
				auto buffer = pExpGetAcsDisp(args[0]);

				WriteFile(pipe, &buffer, sizeof(buffer), &bytesReadOrWritten, nullptr);
			}
			else if (lstrcmp(name, "ExpGetAcsID") == 0)
			{
				auto buffer = pExpGetAcsID(args[0]);

				WriteFile(pipe, &buffer, sizeof(buffer), &bytesReadOrWritten, nullptr);
			}
			else if (lstrcmp(name, "ExpGetAcsMatNum") == 0)
			{
				auto buffer = pExpGetAcsMatNum(args[0]);

				WriteFile(pipe, &buffer, sizeof(buffer), &bytesReadOrWritten, nullptr);
			}

			else if (lstrcmp(name, "ExpGetCurrentObject") == 0)
			{
				auto buffer = pExpGetCurrentObject();

				WriteFile(pipe, &buffer, sizeof(buffer), &bytesReadOrWritten, nullptr);
			}
			else if (lstrcmp(name, "ExpGetCurrentMaterial") == 0)
			{
				auto buffer = pExpGetCurrentMaterial();

				WriteFile(pipe, &buffer, sizeof(buffer), &bytesReadOrWritten, nullptr);
			}
			else if (lstrcmp(name, "ExpGetCurrentTechnic") == 0)
			{
				auto buffer = pExpGetCurrentTechnic();

				WriteFile(pipe, &buffer, sizeof(buffer), &bytesReadOrWritten, nullptr);
			}

			else if (lstrcmp(name, "ExpGetRenderRepeatCount") == 0)
			{
				auto buffer = pExpGetRenderRepeatCount();

				WriteFile(pipe, &buffer, sizeof(buffer), &bytesReadOrWritten, nullptr);
			}

			FlushFileBuffers(pipe);
		}
	}

	CloseHandle(pipe);

	return 0;
}