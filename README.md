# MozaSDK-CSharp
C# Examples of using the MozaSDK

## Features
* Simple C# example to demontrate basic API usage
* Python Wrapper using pythonnet
* Modified example manual_control_steeringwheel.py, using MozaSDK wrapper within [Carla Simulator](https://carla.org/)

## Functions 
* Activate spring force (Wheel centering)
* Get steering, brake, throttle commands
* Get switch status (Data from [Moza Stalks](https://us.mozaracing.com/collections/stalks))

## Install
Note: The core MozaSDK C# files are not including in the repo and must be downloaded from Moza directly

* Download MozaSDK from here [MozaSDK](https://us.mozaracing.com/pages/sdk?utm_source=globalsite&utm_medium=geo_redirect)
* Extract and place MOZA_API_C.dll & MOZA_API_CSharp.dll from the MOZA_SDK\SDK_CSharp folder into your c# solution folder
* Build the c# solution as a release (Creates wrapper DLL)

  ## Notes
  * Spring force can be tuned (increased / decreased) using Moza Pit House
    * Wheel base -> More -> Advanced Settings -> Game Spring Value

