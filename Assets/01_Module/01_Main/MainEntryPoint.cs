using UnityEngine;
using UnityEngine.UI;

public class MainEntryPoint : MonoBehaviour
{
    [SerializeField] SOHub _sOHub;
    [SerializeField] MainUpdateGateway _mainUpdateGateway;
    [SerializeField] MapController _mapController;
    [SerializeField] ComplantView _complantView;
    [SerializeField] Button _goToComplantSubmit;
    [SerializeField] ComplantSubmit _complantSubmit;
    [SerializeField] DonePopup _donePopup;
    void Start()
    {
        //SupabaseUtillity.TestUpload().Cancel();
        //SupabaseUtillity.TestImgUpload(texture).Cancel();
        //SupabaseUtillity.TestImgDownload(tex => rawImage.texture = tex).Cancel();
        //SupabaseUtillity.TestSelect(null).Cancel();
        _sOHub.MainUpdateGateway = _mainUpdateGateway;
        _sOHub.MapController = _mapController;
        _sOHub.ComplantView = _complantView;
        _sOHub.ComplantSubmit = _complantSubmit;
        _sOHub.DonePopup = _donePopup;

        _mapController.Init();
        _complantView.Init();
        _complantSubmit.Init();
        _donePopup.Init();

        _goToComplantSubmit.onClick.AddListener(_complantSubmit.On);

        _mainUpdateGateway.Run();
    }
}
