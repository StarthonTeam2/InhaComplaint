using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;

public class InitEntryPoint : MonoBehaviour
{
    [SerializeField] SOHub _sOHub;

    async Awaitable Start()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);

            while (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                await Awaitable.NextFrameAsync();
            }
        }
        PermissionUtility.RequestLocationPermission();
        Input.location.Start();
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            await Awaitable.WaitForSecondsAsync(1);
            maxWait--;
        }

        if (maxWait < 1)
        {
            Debug.LogError("위치 서비스 초기화 시간 초과");
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.LogError("위치 정보를 가져오는 데 실패했습니다.");
        }

        LocationInfo locationData = Input.location.lastData;
        SupabaseUtillity.Select(json => SupabaseUtillity.SettingData(json, _sOHub, () => SceneManager.LoadScene("Main"))).Cancel();
    }
}
