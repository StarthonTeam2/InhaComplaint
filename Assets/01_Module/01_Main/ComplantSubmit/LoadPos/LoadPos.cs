using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadPos : MonoBehaviour
{
    [SerializeField] Button _button;
    [SerializeField] TMP_Text _addr;

    bool _isDone;
    internal bool IsDone => _isDone;
    internal string Addr => _addr.text;
    internal float Lat { get; private set; }
    internal float Lon { get; private set; }

    internal void Init()
    {
        _button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        _button.interactable = false;
        LocationInfo location = Input.location.lastData;
        if (location.latitude == 0) { _addr.text = "GPS를 확인해주세요"; _button.interactable = true; }
        Lat = location.latitude; Lon = location.longitude;
        //Lat = 37.440141701381535f; Lon = 126.69443001176306f;
        NaverMapUtillity.ReverseGeocoding(Lat, Lon, Callback).Cancel();
    }

    internal void Callback(string addr)
    {
        if (string.IsNullOrWhiteSpace(addr))
        {
            _isDone = false;
            _addr.text = "GPS를 확인해주세요";
        }
        else
        {
            _isDone = true;
            _addr.text = addr;
        }
        _button.interactable = true;
    }
}
