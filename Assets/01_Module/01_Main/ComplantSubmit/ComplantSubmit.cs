using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComplantSubmit : MonoBehaviour
{
    [SerializeField] SOHub _sOHub;

    [SerializeField] Button _exit;

    [SerializeField] TMP_Text _imageTextHighlight;
    [SerializeField] ImageInput _imageInput;
    string _imageFullpath;

    [SerializeField] TMP_Text _loadPosTextHighlight;
    [SerializeField] LoadPos _loadPos;

    [SerializeField] TMP_InputField _inputField;

    [SerializeField] Button _submit;

    [SerializeField] GameObject _screenRock;

    bool _isOn;
    internal bool IsOn => _isOn;

    internal void Init()
    {
        _exit.onClick.AddListener(() => { _isOn = false; gameObject.SetActive(false); });

        _imageInput.Init();
        _imageInput.OnSelectImage += imageFullpath =>_imageFullpath = imageFullpath;
        _imageInput.OnDeselectImage += () => _imageFullpath = null;
        _loadPos.Init();

        _submit.onClick.AddListener(Submit);
    }

    internal void On()
    {
        _isOn = true;
        _screenRock.SetActive(false);
        _imageTextHighlight.text = "*";
        _imageInput.OnImageDeselect();
        _loadPosTextHighlight.text = "*";
        _loadPos.Callback(null);
        _inputField.text = "";
        gameObject.SetActive(true);
    }

    void Submit()
    {
        //이것저것체크 충족시 올리고 아니면 경고
        bool submitable = true;
        if (_imageFullpath == null)
        {
            submitable &= false;
            _imageTextHighlight.text = "사진을 첨부하세요!";
        }
        if (!_loadPos.IsDone)
        {
            submitable &= false;
            _loadPosTextHighlight.text = "위치를 불러오세요!";
        }

        if (submitable)
        {
            _screenRock.SetActive(true);
            SupabaseUtillity.ImageUpload(_imageFullpath, RealSubmit).Cancel();
        }
    }

    void RealSubmit(string imgPath)
    {
        RequestSubmit(imgPath).Cancel();

        async Awaitable RequestSubmit(string imgPath)
        {
            await SupabaseUtillity.UploadRDBRow(new ComplantRDBDTO()
            {
                lat = _loadPos.Lat,
                lon = _loadPos.Lon,
                addr = _loadPos.Addr,
                content = _inputField.text,
                img_link = imgPath
            });

            SupabaseUtillity.Select(DownLoadComplant).Cancel();

            void DownLoadComplant(string json)
            {
                SupabaseUtillity.SettingData(json, _sOHub, () =>
                {
                    _sOHub.MapController.Resetting(_loadPos.Lat, _loadPos.Lon);
                    _sOHub.DonePopup.On(_loadPos.Addr);
                    _screenRock.SetActive(false);
                    _isOn = false;
                    gameObject.SetActive(false);
                });
            }
        }
    }
}
