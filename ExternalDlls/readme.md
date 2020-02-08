Here must be folders:
* ```Unity3D/``` - unity3d's dlls
* ```VRCSDK/``` - vrchat sdk
* ```Udon/``` - udon sdk

It's all used by udon external api generator library, so if there's some dll's missing - api generator would not compile

## Unity3D/

Copy Unity3D dll's from unity installation folder (it must be the same unity version you use for udon development!)
Dll's, that has to be copied:
*  ```%UnityInstallation%\Editor\Data\Managed\UnityEngine.dll```
*  ```%UnityInstallation%\Editor\Data\UnityExtensions\Unity\GUISystem\UnityEngine.UI.dll```

```%UnityInstallation%``` is ```C:\Program Files\Unity\Hub\Editor\2018.4.14f1\``` in my case (it's not path variable i'm just using it here for convinience, you have to figure out what your installation path by your own)

## VRCSDK/

Install Vrchat SDK 3 (not 2, as it is not compatible with udon) to some unity project (extract unity package for that) - as result it would create ```VRCSDK``` folder under your project's assets folder - just copy this whole folder.

## Udon/

Same as for ```VRCSDK/```

