using UnityEngine;

public abstract class AddOn : Photon.MonoBehaviour
{
    public string addOnName;
    public PhotonView PhotonView { get; protected set; }
    public abstract void StartAddOn();
}