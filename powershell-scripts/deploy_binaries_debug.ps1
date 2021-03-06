Write-Output "BEGIN: deploy binaries (debug)"

$compilerBinFolder = "../DoshikLangCompiler/bin/Debug/netstandard2.0"
$apiGeneratorBinFolder = "../DoshikExternalApiGenerator/bin/Debug/netstandard2.0"
$apiCacheBinFolder = "../DoshikExternalApiCache/bin/Debug/netstandard2.0"
$irBinFolder = "../DoshikLangIR/bin/Debug/netstandard2.0"
$apiBinFolder = "../DoshikExternalApi/bin/Debug/netstandard2.0"
$langServerBinFolder = "../DoshikLanguageServer/bin/Debug/netcoreapp3.1"
$dependenciesBinFolder = "../Tester/bin/Debug"
$syntaxHighlightFolder = "../syntax-highlight"

$doshikSdkExternalDllsFolder = "../DoshikSDKAssets/Assets/DoshikSDK/Editor/ExternalDlls"

$vsCodeExtensionsDoshikFolder = "../vscode-extensions/doshik"
$vsCodeExtensionsDoshikServerFolder = $vsCodeExtensionsDoshikFolder + "/server"
$vsCodeExtensionsDoshikSyntaxesFolder = $vsCodeExtensionsDoshikFolder + "/syntaxes"

Remove-Item ($doshikSdkExternalDllsFolder + "/*") -Force -Recurse -ErrorAction SilentlyContinue
New-Item -ErrorAction Ignore -ItemType directory -Path $doshikSdkExternalDllsFolder

Remove-Item ($vsCodeExtensionsDoshikServerFolder + "/*") -Force -Recurse -ErrorAction SilentlyContinue
New-Item -ErrorAction Ignore -ItemType directory -Path $vsCodeExtensionsDoshikServerFolder

Remove-Item ($vsCodeExtensionsDoshikSyntaxesFolder + "/*") -Force -Recurse -ErrorAction SilentlyContinue
New-Item -ErrorAction Ignore -ItemType directory -Path $vsCodeExtensionsDoshikSyntaxesFolder

Copy-Item -Path ($apiBinFolder + "/DoshikExternalApi.dll") -Destination $doshikSdkExternalDllsFolder
Copy-Item -Path ($apiGeneratorBinFolder + "/DoshikExternalApiGenerator.dll") -Destination $doshikSdkExternalDllsFolder
Copy-Item -Path ($apiCacheBinFolder + "/DoshikExternalApiCache.dll") -Destination $doshikSdkExternalDllsFolder
Copy-Item -Path ($irBinFolder + "/DoshikLangIR.dll") -Destination $doshikSdkExternalDllsFolder
Copy-Item -Path ($compilerBinFolder + "/DoshikLangCompiler.dll") -Destination $doshikSdkExternalDllsFolder

Copy-Item -Path ($dependenciesBinFolder + "/Antlr4.Runtime.Standard.dll") -Destination $doshikSdkExternalDllsFolder
Copy-Item -Path ($dependenciesBinFolder + "/Newtonsoft.Json.dll") -Destination $doshikSdkExternalDllsFolder

Copy-Item -Path ($langServerBinFolder + "/*") -Destination $vsCodeExtensionsDoshikServerFolder

Copy-Item -Path ($syntaxHighlightFolder + "/doshik.tmLanguage") -Destination $vsCodeExtensionsDoshikSyntaxesFolder

Push-Location
Set-Location $vsCodeExtensionsDoshikFolder
npm install
Pop-Location

Write-Output "END: deploy binaries (debug)"