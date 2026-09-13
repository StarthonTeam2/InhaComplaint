using UnityEngine;
using UnityEngine.Android;

public class PermissionUtility : MonoBehaviour
{
    internal static void FullRequestLocationSetting()
    {
        print(Permission.HasUserAuthorizedPermission(Permission.FineLocation) + " : 허용상태");
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Debug.Log("아직 허용 안됨");
            RequestLocationSetting();
        }
        else { Debug.Log("허용되어있음"); }
    }

    internal static void RequestLocationSetting()
    {
        PermissionCallbacks callbacks = new PermissionCallbacks();
        callbacks.PermissionGranted += permission => { Debug.Log("권한 허용"); };
        callbacks.PermissionDenied += permission => { Debug.Log("권한 거부"); };
        callbacks.PermissionRequestDismissed += permission => { Debug.Log("다시 묻지 않음"); };
        Permission.RequestUserPermission(Permission.FineLocation, callbacks);
    }

    internal static void RequestLocationPermission()
    {
        using var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        using var plugin = new AndroidJavaClass("com.terrrrrrra.gpsplugin.GpsSettingsPlugin");
        plugin.CallStatic("ShowGpsDialog", activity);
    }

    internal static bool IsGetLocation()
    {
        print($"{Input.location.isEnabledByUser} | {Permission.HasUserAuthorizedPermission(Permission.FineLocation)} | {Input.location.status}");
        return Input.location.isEnabledByUser && Permission.HasUserAuthorizedPermission(Permission.FineLocation);
    }
}
