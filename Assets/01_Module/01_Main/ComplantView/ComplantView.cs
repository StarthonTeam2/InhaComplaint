using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComplantView : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] Button _exit;

    [SerializeField] RectTransform _beforeImageMask;
    [SerializeField] RawImage _beforeImage;

    [SerializeField] GameObject _afterText;
    [SerializeField] RectTransform _afterImageMask;
    [SerializeField] RawImage _afterImage;

    [SerializeField] GameObject _beforeProcess;
    [SerializeField] GameObject _afterProcess;

    [SerializeField] TMP_Text _posText;
    [SerializeField] TMP_Text _dateText;

    [SerializeField] TMP_Text _complaintText1;
    [SerializeField] TMP_Text _complaintText2;

    bool _isOn;
    internal bool IsOn => _isOn;

    internal void Init()
    {
        _exit.onClick.AddListener(() => { _animator.SetTrigger("Hide"); _isOn = false; });
    }

    internal void On(ComplantDTO dTO)
    {
        _animator.SetTrigger("Show");
        _isOn = true;
        float ratio = (float)dTO.img.height / dTO.img.width;
        _beforeImageMask.sizeDelta = new Vector2(_beforeImageMask.sizeDelta.x, _beforeImageMask.sizeDelta.x * ratio);
        _beforeImage.texture = dTO.img;

        _afterText.SetActive(dTO.done);
        _afterImageMask.gameObject.SetActive(dTO.done);
        if (dTO.done)
        {
            ratio = (float)dTO.doneImg.height / dTO.doneImg.width;
            _afterImageMask.sizeDelta = new Vector2(_afterImageMask.sizeDelta.x, _afterImageMask.sizeDelta.x * ratio);

            _afterImage.texture = dTO.doneImg;
        }

        _beforeProcess.SetActive(!dTO.done);
        _afterProcess.SetActive(dTO.done);

        _posText.text = dTO.addr;
        _dateText.text = dTO.dateTime;

        _complaintText1.text = dTO.content;
        _complaintText2.text = dTO.content;
    }
}
