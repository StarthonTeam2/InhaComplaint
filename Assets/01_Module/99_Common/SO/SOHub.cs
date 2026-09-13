using UnityEngine;

[CreateAssetMenu(fileName = "SOHub", menuName = "Scriptable Objects/SOHub")]
public class SOHub : ScriptableObject
{
    internal MainUpdateGateway MainUpdateGateway;
    internal ComplantView ComplantView;
    internal ComplantDTO[] ComplantDTOArr;

}
