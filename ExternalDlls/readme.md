# external .dll files needed to collect udon graph api data outside of unity3d

## files from unity3d installation
```%UnityInstallation%``` is ```C:\Program Files\Unity\Hub\Editor\2018.4.14f1\``` in my case

*  ```%UnityInstallation%\Editor\Data\Managed\UnityEngine.dll```
*  ```%UnityInstallation%\Editor\Data\UnityExtensions\Unity\GUISystem\UnityEngine.UI.dll```

## files from VRCSDK3 and Udon SDK
```%UnityProject%``` in my case is just path to some unity3d project with imported SDK's in it

* ```%UnityProject%\Assets\Udon\External\```
    * ```VRC.Udon.ClientBindings.dll```
    * ```VRC.Udon.Common.dll```
    * ```VRC.Udon.Security.dll```
    * ```VRC.Udon.VM.dll```
    * ```VRC.Udon.Wrapper.dll```

* ```%UnityProject%\Assets\Udon\Editor\External\```
    * ```VRC.Udon.Compiler.dll```
    * ```VRC.Udon.EditorBindings.dll```
    * ```VRC.Udon.Graph.dll```
    * ```VRC.Udon.UAssembly.dll```

* ```%UnityProject%\Assets\VRCSDK\Plugins\```
    * ```VRCCore-Editor.dll```
    * ```VRCCore-Standalone.dll```
    * ```VRCSDK3.dll```
    * ```VRCSDK3-Editor.dll```
    * ```VRCSDKBase.dll```