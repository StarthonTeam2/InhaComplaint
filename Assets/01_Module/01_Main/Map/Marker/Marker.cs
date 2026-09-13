using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Marker : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] SOHub _sOHub;
    [SerializeField] Image _outLine;
    [SerializeField] RawImage _rawImage;
    [SerializeField] RectTransform _rectTransform;
    ComplantDTO _complantDTO;
    Vector2 _startPos;

    internal void Init(ComplantDTO dTO)
    {
        _outLine.color = dTO.done ? Color.green : Color.red;
        _rawImage.texture = dTO.done ? dTO.doneImg : dTO.img;
        _complantDTO = dTO;
    }

    internal void SetStartPos(in Vector2 centerPos, int level)
    {
        float worldSize = 1024f * Mathf.Pow(2, level);

        float latRad = centerPos.y * Mathf.Deg2Rad;

        float pixelsPerDegreeLon = worldSize / 360f;
        float pixelsPerDegreeLat = pixelsPerDegreeLon / Mathf.Cos(latRad);

        float dx = (_complantDTO.lon - centerPos.x) * pixelsPerDegreeLon;
        float dy = (_complantDTO.lat - centerPos.y) * pixelsPerDegreeLat;

        _startPos = new Vector2(dx, dy);
        _rectTransform.anchoredPosition = _startPos;
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        _sOHub.ComplantView.On(_complantDTO);
    }
}
