Write-Output "BEGIN: deploy vs code extension"

$vsCodeExtensionsFolder = $env:USERPROFILE + "/.vscode/extensions"
$vsCodeExtensionsDoshikFolder = $vsCodeExtensionsFolder + "/doshik"

$vsCodeExtensionsDoshikToCopyFolder = "../vscode-extensions/doshik"

Remove-Item $vsCodeExtensionsDoshikFolder -Force -Recurse -ErrorAction SilentlyContinue

Copy-Item $vsCodeExtensionsDoshikToCopyFolder -Destination $vsCodeExtensionsDoshikFolder -Recurse

Write-Output "END: deploy vs code extension"