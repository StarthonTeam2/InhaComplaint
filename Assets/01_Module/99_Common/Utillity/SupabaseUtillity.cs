using System;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class SupabaseUtillity
{
    static string url = "https://kbhymsvfrtdrhpqvtecp.supabase.co";
    static string key = "sb_publishable_oDMNfSjNZIUUTXMaFg_gaA_9e9WGs2P";

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
    
    internal static async Awaitable ImageUpload(Texture2D texture, string extension, Action<string> callback)
    {
        string filename = $"{Guid.NewGuid()}{extension}";
        using UnityWebRequest www = UnityWebRequest.Post(url + $"/storage/v1/object/images/{filename}", new WWWForm());
        www.uploadHandler = new UploadHandlerRaw(texture.EncodeToPNG());
        www.SetRequestHeader("Content-Type", "image/png");
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
}
