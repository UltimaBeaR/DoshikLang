Write-Output "BEGIN: deploy unity project"

if (-not (Test-Path env:DOSHIKUNITYPROJECT)) {
    Write-Error "END: DOSHIKUNITYPROJECT path variable doesn't exist"

    exit
}

$assetsFolder = $env:DOSHIKUNITYPROJECT + "/Assets"
$doshikSdkFolder = $assetsFolder + "/DoshikSDK"
$doshikSdkToCopyFolder = "../DoshikSDKAssets/Assets/DoshikSDK"

Remove-Item $doshikSdkFolder -Force -Recurse -ErrorAction SilentlyContinue

Copy-Item $doshikSdkToCopyFolder -Destination $assetsFolder -Recurse

Write-Output "END: deploy unity project"