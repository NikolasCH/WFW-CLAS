using UnityEngine;
using System.Collections;

public class ReSize : MonoBehaviour
{

    // Use this for initialization
    private void Awake()
    {
        if (UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPad2Gen ||
            UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPad3Gen ||
            UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPad4Gen ||
            UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPad5Gen ||
            UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPad6Gen ||
            UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPadAir1 ||
            UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPadAir2 ||
            UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPadMini1Gen ||
            UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPadMini2Gen ||
            UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPadMini3Gen ||
            UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPadMini4Gen ||
            Screen.width == 2732)
            transform.localScale = new Vector3(0.8f, 0.8f, 1);

        Debug.Log(UnityEngine.iOS.Device.generation);
    }

}
