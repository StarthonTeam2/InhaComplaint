using UnityEngine;

[CreateAssetMenu(fileName = "SOHub", menuName = "Scriptable Objects/SOHub")]
public class SOHub : ScriptableObject
{
    internal MainUpdateGateway MainUpdateGateway;
    internal MapController MapController;
    internal ComplantView ComplantView;
    internal ComplantSubmit ComplantSubmit;
    internal DonePopup DonePopup;
    internal ComplantDTO[] ComplantDTOArr;

}
