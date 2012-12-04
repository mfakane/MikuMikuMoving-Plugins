// stdafx.h : 標準のシステム インクルード ファイルのインクルード ファイル、または
// 参照回数が多く、かつあまり変更されない、プロジェクト専用のインクルード ファイル
// を記述します。
//

#pragma once

#include "targetver.h"

#define WIN32_LEAN_AND_MEAN             // Windows ヘッダーから使用されていない部分を除外します。
// Windows ヘッダー ファイル:
#include <windows.h>

#undef RtlMoveMemory
#undef RtlFillMemory
#undef RtlZeroMemory
EXTERN_C NTSYSAPI VOID NTAPI RtlMoveMemory (LPVOID UNALIGNED Dst, LPCVOID UNALIGNED Src, SIZE_T Length);
EXTERN_C NTSYSAPI VOID NTAPI RtlFillMemory (LPVOID UNALIGNED Dst, SIZE_T Length, BYTE Pattern);
EXTERN_C NTSYSAPI VOID NTAPI RtlZeroMemory (LPVOID UNALIGNED Dst, SIZE_T Length);

// TODO: プログラムに必要な追加ヘッダーをここで参照してください。
#include "MMDExport.h"