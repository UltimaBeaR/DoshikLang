Write-Output "BEGIN: deploy binaries (debug)"

$compilerBinFolder = "../DoshikLangCompiler/bin/Debug/netstandard2.0"
$apiGeneratorBinFolder = "../DoshikExternalApiGenerator/bin/Debug/netstandard2.0"
$apiCacheBinFolder = "../DoshikExternalApiCache/bin/Debug/netstandard2.0"
$irBinFolder = "../DoshikLangIR/bin/Debug/netstandard2.0"
$apiBinFolder = "../DoshikExternalApi/bin/Debug/netstandard2.0"
$langServerBinFolder = "../DoshikLanguageServer/bin/Debug/netcoreapp3.1"
$dependenciesBinFolder = "../Tester/bin/Debug"

$doshikSdkExternalDllsFolder = "../DoshikSDKAssets/Assets/DoshikSDK/Editor/ExternalDlls"
$vsCodeExtensionsServerFolder = "../vscode-extensions/doshik/server"

Remove-Item ($doshikSdkExternalDllsFolder + "/*")
Remove-Item ($vsCodeExtensionsServerFolder + "/*")

Copy-Item -Path ($apiBinFolder + "/DoshikExternalApi.dll") -Destination $doshikSdkExternalDllsFolder
Copy-Item -Path ($apiGeneratorBinFolder + "/DoshikExternalApiGenerator.dll") -Destination $doshikSdkExternalDllsFolder
Copy-Item -Path ($apiCacheBinFolder + "/DoshikExternalApiCache.dll") -Destination $doshikSdkExternalDllsFolder
Copy-Item -Path ($irBinFolder + "/DoshikLangIR.dll") -Destination $doshikSdkExternalDllsFolder
Copy-Item -Path ($compilerBinFolder + "/DoshikLangCompiler.dll") -Destination $doshikSdkExternalDllsFolder

Copy-Item -Path ($dependenciesBinFolder + "/Antlr4.Runtime.Standard.dll") -Destination $doshikSdkExternalDllsFolder
Copy-Item -Path ($dependenciesBinFolder + "/Newtonsoft.Json.dll") -Destination $doshikSdkExternalDllsFolder

Copy-Item -Path ($langServerBinFolder + "/*") -Destination $vsCodeExtensionsServerFolder

Write-Output "END: deploy binaries (debug)"