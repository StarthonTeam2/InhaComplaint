using System;

[Serializable]
public class ComplantRDBDTO
{
    public int id;
    public string created_at;
    public float lat;
    public float lon;
    public string addr;
    public string content;
    public string img_link;
    public bool done;
    public string done_img_link;
}

[Serializable]
public class ComplantRDBDTOGroup
{
    public ComplantRDBDTO[] dtoArr;
}
