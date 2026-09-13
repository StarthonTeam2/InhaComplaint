using UnityEngine;

public class MainEntryPoint : MonoBehaviour
{
    [SerializeField] SOHub _sOHub;
    [SerializeField] MainUpdateGateway _mainUpdateGateway;
    [SerializeField] MapController _mapController;
    [SerializeField] ComplantView _complantView;
    void Start()
    {
        //SupabaseUtillity.TestUpload().Cancel();
        //SupabaseUtillity.TestImgUpload(texture).Cancel();
        //SupabaseUtillity.TestImgDownload(tex => rawImage.texture = tex).Cancel();
        //SupabaseUtillity.TestSelect(null).Cancel();
        _sOHub.MainUpdateGateway = _mainUpdateGateway;
        _sOHub.ComplantView = _complantView;

        _mapController.Init();
        _complantView.Init();

        _mainUpdateGateway.Run();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
