#!/usr/bin/env pwsh

# Copyright 2020 pixiv Inc. All Rights Reserved.
#
# Use of this source code is governed by a license that can be
# found in the LICENSE.pixiv file in the root of the source tree.

New-Item depot_tools -ItemType directory
Set-Location depot_tools
git init
git fetch --depth=1 https://chromium.googlesource.com/chromium/tools/depot_tools.git 0ba2fd429dd6db431fcbee6995c1278d2a3657a0
git checkout FETCH_HEAD
Write-Output "$pwd" | Out-File -FilePath $env:GITHUB_PATH -Encoding utf8 -Append
