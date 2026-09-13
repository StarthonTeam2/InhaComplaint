using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class SupabaseUtillity
{
    static string url = "https://kbhymsvfrtdrhpqvtecp.supabase.co";
    static string key = "sb_publishable_oDMNfSjNZIUUTXMaFg_gaA_9e9WGs2P";

    static SOHub _sOHub;
    static Action _callback;

    static int _done, _hurdle;
    static Dictionary<int, ComplantDTO> _complantDTODict = new Dictionary<int, ComplantDTO>();

    internal static async Awaitable UploadRDBRow(ComplantRDBDTO dTO)
    {
        string json = $"{{\"lat\":\"{dTO.lat}\",\"lon\":\"{dTO.lon}\",\"addr\":\"{dTO.addr}\",\"content\":\"{dTO.content}\",\"img_link\":\"{dTO.img_link}\"}}";

        using UnityWebRequest www = UnityWebRequest.Post(url + "/rest/v1/Complaint", new WWWForm());
        www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("apikey", key);

        await www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("등록 성공!");
            Debug.Log(www.downloadHandler.text);
        }
        else
        {
            Debug.LogError(
                $"등록 실패: {www.responseCode}\n" +
                www.downloadHandler.text
            );
        }
    }
    
    internal static async Awaitable ImageUpload(string fullpath, Action<string> callback)
    {
        string extension = Path.GetExtension(fullpath);
        string filename = $"{Guid.NewGuid()}{extension}";
        using UnityWebRequest www = UnityWebRequest.Post(url + $"/storage/v1/object/images/{filename}", new WWWForm());
        byte[] data;
        string contentType;
        data = File.ReadAllBytes(fullpath);
        if (extension.ToLower().Contains("jpg"))
        {
            contentType = "image/jpg";
        }
        else
        {
            contentType = "image/png";
        }
        www.uploadHandler = new UploadHandlerRaw(data);
        www.SetRequestHeader("Content-Type", contentType);
        www.SetRequestHeader("apikey", key);
        await www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("등록 성공!");
            callback?.Invoke(filename);
        }
        else
        {
            Debug.LogError(
                $"등록 실패: {www.responseCode}\n" +
                www.downloadHandler.text
            );
            callback?.Invoke(null);
        }
    }

    internal static async Awaitable Select(Action<string> callback)
    {
        using UnityWebRequest www = UnityWebRequest.Get(url + $"/rest/v1/Complaint?select=*");
        www.SetRequestHeader("apikey", key);
        await www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.Success)
        {
            callback?.Invoke(www.downloadHandler.text);
        }
        else
        {
            callback?.Invoke(null);
        }
    }

    internal static async Awaitable ImageDownload(string filename, Action<Texture, int, bool> callback, int id, bool isDoneImg)
    {
        using UnityWebRequest www = UnityWebRequestTexture.GetTexture(url + $"/storage/v1/object/images/{filename}");
        await www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.Success)
        {
            callback?.Invoke(DownloadHandlerTexture.GetContent(www), id, isDoneImg);
        }
        else
        {
            callback?.Invoke(null, id, isDoneImg);
        }
    }

    internal static void SettingData(string json, SOHub sOHub, Action callback)
    {
        _sOHub = sOHub;
        _callback = callback;

        ComplantRDBDTOGroup complantDTOGroup = JsonUtility.FromJson<ComplantRDBDTOGroup>($"{{\"dtoArr\":{json}}}");
        _done = 0; _hurdle = 0;
        for (int i = 0; i < complantDTOGroup.dtoArr.Length; i++)
        {
            if (!_complantDTODict.ContainsKey(complantDTOGroup.dtoArr[i].id))
            {
                DownloadImg(complantDTOGroup.dtoArr[i]);
            }
        }
    }

    static void DownloadImg(ComplantRDBDTO dto)
    {
        _hurdle += dto.done ? 2 : 1;
        _complantDTODict[dto.id] = new ComplantDTO(dto);
        ImageDownload(dto.img_link, DownloadDone, dto.id, false).Cancel();
        if (dto.done)
        {
            ImageDownload(dto.done_img_link, DownloadDone, dto.id, true).Cancel();
        }
    }

    static void DownloadDone(Texture tex, int id, bool isDoneImg)
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
        if (_done == _hurdle)
        {
            _sOHub.ComplantDTOArr = _complantDTODict.Select(pair => pair.Value).ToArray();
            _callback?.Invoke();
        }
    }
}
