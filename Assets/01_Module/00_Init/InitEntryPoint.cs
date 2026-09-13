using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;

public class InitEntryPoint : MonoBehaviour
{
    [SerializeField] SOHub _sOHub;
    int _done, _hurdle;
    Dictionary<int, ComplantDTO> _complantDTODict = new Dictionary<int, ComplantDTO>();

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
        Debug.Log($"위도: {locationData.latitude}, 경도: {locationData.longitude}");
        SupabaseUtillity.Select(DownLoadComplant).Cancel();
        //SceneManager.LoadScene("Main");
    }

    void DownLoadComplant(string json)
    {
        ComplantRDBDTOGroup complantDTOGroup = JsonUtility.FromJson<ComplantRDBDTOGroup>($"{{\"dtoArr\":{json}}}");
        _done = 0; _hurdle = 0; _complantDTODict.Clear();
        for (int i = 0; i < complantDTOGroup.dtoArr.Length; i++)
        {
            DownloadImg(complantDTOGroup.dtoArr[i]);
        }
    }

    void DownloadImg(ComplantRDBDTO dto)
    {
        _hurdle += dto.done ? 2 : 1;
        _complantDTODict[dto.id] = new ComplantDTO(dto);
        SupabaseUtillity.ImageDownload(dto.img_link, DownloadDone, dto.id, false).Cancel();
        if (dto.done)
        {
            SupabaseUtillity.ImageDownload(dto.done_img_link, DownloadDone, dto.id, true).Cancel();
        }
    }

    void DownloadDone(Texture tex, int id, bool isDoneImg)
    {
        if (isDoneImg)
        {
            _complantDTODict[id].doneImg = tex;
        }
        else
        {
            _complantDTODict[id].img = tex;
        }
        _done++;
        if(_done == _hurdle)
        {
            _sOHub.ComplantDTOArr = _complantDTODict.Select(pair => pair.Value).ToArray();
            SceneManager.LoadScene("Main");
        }
    }
}
