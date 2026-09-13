using System;
using UnityEngine;
using UnityEngine.Networking;

public class NaverMapUtillity
{
    static string url = "https://maps.apigw.ntruss.com/map-static/v2";
    static string id = "wrpb0ts4ij";
    static string key = "L1geSgmoLD8EUYCG5TazTsgmVQacpufpOAJH8YqW";

    internal static async Awaitable DownloadMap(StaticMapInfoDTO dTO, int token, Action<int, Texture> callback)
    {
        string fullUrl = $"{url}/raster?w={dTO.w}&h={dTO.h}&center={dTO.lon},{dTO.lat}&level={dTO.level}&scale=2";
        using UnityWebRequest www = UnityWebRequestTexture.GetTexture(fullUrl);
        www.SetRequestHeader("x-ncp-apigw-api-key-id", id);
        www.SetRequestHeader("x-ncp-apigw-api-key", key);
        await www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            callback?.Invoke(token, DownloadHandlerTexture.GetContent(www));
        }
        else
        {
            callback?.Invoke(token, null);
        }
        //Debug.Log(www.responseCode + " " + www.downloadHandler.text);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
