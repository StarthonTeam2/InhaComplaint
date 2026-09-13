using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class MapController : MonoBehaviour, IUpdate
{
    [SerializeField] SOHub _sOHub;
    [SerializeField] RawImage _map;
    [SerializeField] Marker _markerPrefab;
    [SerializeField] RectTransform _markerRoot;

    List<Marker> _markerList = new List<Marker>();

    Vector2 _lastPos;
    bool _isTouch0Press;
    bool _isTouch1Perform;
    TouchState _touch0;
    TouchState _touch1;

    int _level = 16;

    MainInput _mainInput;
    internal void Init()
    {
        _sOHub.MainUpdateGateway.Add(this);
        _mainInput = new MainInput();
        _mainInput.Enable();
        LocationInfo locationData = Input.location.lastData;
        _lastPos = new Vector2(126.6580320932838f, 37.451520869041815f/*locationData.latitude, locationData.longitude*/);//위도 위치로

        for (int i = 0; i < _sOHub.ComplantDTOArr.Length; i++)
        {
            Marker curMarker = Instantiate(_markerPrefab, _markerRoot);
            curMarker.transform.localScale = Vector3.one;
            curMarker.Init(_sOHub.ComplantDTOArr[i]);
            _markerList.Add(curMarker);
        }

        UpdateMap();
    }

    void IUpdate.Update()
    {
        if (_sOHub.ComplantView.IsOn) { return; }

        _touch0 = _mainInput.Pointer.Touch0.ReadValue<TouchState>();
        _touch1 = _mainInput.Pointer.Touch1.ReadValue<TouchState>();

        if (_isTouch0Press)
        {
            if (_isTouch1Perform)
            {

            }
            else
            {
                Vector2 delta = new Vector2(_touch0.position.x - _touch0.startPosition.x, _touch0.position.y - _touch0.startPosition.y);
                _map.rectTransform.anchoredPosition = delta;
                _markerRoot.anchoredPosition = delta;
            }
        }

        if (_mainInput.Pointer.Touch0Press.WasPressedThisFrame())
        {
            _isTouch0Press = true;
        }
        if (_mainInput.Pointer.Touch0Press.WasReleasedThisFrame())
        {
            _isTouch0Press = false;
            float worldSize = 1024 * Mathf.Pow(2, _level);

            float latRad = _lastPos.y * Mathf.PI / 180;
            float mercatorY = Mathf.Log(Mathf.Tan(Mathf.PI / 4 + latRad / 2));
            mercatorY -= (_touch0.position.y - _touch0.startPosition.y) / worldSize * 2 * Mathf.PI;
            float newLatRad = 2 * Mathf.Atan(Mathf.Exp(mercatorY)) - Mathf.PI / 2;
            float newLatitude = newLatRad * 180 / Mathf.PI;

            float newLongitude = _lastPos.x + (_touch0.startPosition.x - _touch0.position.x) / worldSize * 360;

            _lastPos.y = newLatitude;
            _lastPos.x = newLongitude;
            UpdateMap();
        }
        if (_mainInput.Pointer.Touch1Press.WasPressedThisFrame())
        {
            _isTouch1Perform = true;
        }
        if (_mainInput.Pointer.Touch1Press.WasReleasedThisFrame())
        {
            _isTouch1Perform = false;
        }
    }

    void UpdateMap()
    {
        NaverMapUtillity.DownloadMap(new StaticMapInfoDTO()
        {
            w = Mathf.RoundToInt(_map.rectTransform.rect.width),
            h = Mathf.RoundToInt(_map.rectTransform.rect.height),
            lat = _lastPos.y,
            lon = _lastPos.x,
            level = _level
        }, int.MinValue, (a, b) => { _map.texture = b; _map.rectTransform.anchoredPosition = Vector2.zero; }).Cancel();
        _markerRoot.anchoredPosition = Vector2.zero;
        for (int i = 0; i < _markerList.Count; i++)
        {
            _markerList[i].SetStartPos(in _lastPos, _level);
        }
    }
}
