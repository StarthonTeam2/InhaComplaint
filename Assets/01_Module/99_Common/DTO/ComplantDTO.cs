using UnityEngine;

public class ComplantDTO
{
    public string dateTime;
    public float lat;
    public float lon;
    public string addr;
    public string content;
    public Texture img;
    public bool done;
    public Texture doneImg;
    public ComplantDTO(ComplantRDBDTO dTO)
    {
        dateTime = dTO.created_at.Substring(0, 10);
        lat = dTO.lat;
        lon = dTO.lon;
        addr = dTO.addr;
        content = dTO.content;
        done = dTO.done;
    }
}
