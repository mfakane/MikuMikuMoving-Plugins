#pragma once

// SDKDDKVer.h をインクルードすると、利用できる最も上位の Windows プラットフォームが定義されます。

// 以前の Windows プラットフォーム用にアプリケーションをビルドする場合は、WinSDKVer.h をインクルードし、
// SDKDDKVer.h をインクルードする前に、サポート対象とするプラットフォームを示すように _WIN32_WINNT マクロを設定します。

#define NTDDI_VERSION 0x05000000
#define WINVER _WIN32_WINNT
#define _WIN32_WINNT 0x0500
#define _WIN32_WINDOWS _WIN32_WINNT
#define _WIN32_IE 0x0501
#include <SDKDDKVer.h>
