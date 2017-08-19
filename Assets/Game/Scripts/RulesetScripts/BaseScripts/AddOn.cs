using UnityEngine;

public abstract class AddOn : MonoBehaviour
{
    public string addOnName;
    public PhotonView PhotonView { get; protected set; }
    public abstract void StartAddOn();
    public abstract void EndAddOn();
}