using UnityEngine;

public class FlagBase : MonoBehaviour
{
    public Flag flag;
    public PlayerFlagInfo owner;
    public bool hasFlag = true;

    void Start()
    {
        hasFlag = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (owner == null) return; //if they don't have an owner, they cannot be used to capture a flag
        if (other.tag.Equals("Flag")) // if a flag enters the base
        {
            Flag capturedFlag = other.GetComponent<Flag>();
            if (hasFlag) //if the base has its flag
            {
                if (!capturedFlag.index.Equals(flag.index)) //if the flag isn't the flag the base owns
                {
                    RefereeManager.instance.PlayFlagCaptured();
                    FlagManager.instance.Local_ReturnFlag(other.GetComponent<Flag>().index);
                    FlagManager.instance.photonView.RPC("RPC_ReturnFlag", PhotonTargets.Others, other.GetComponent<Flag>().index);
                    hasFlag = false;
                }
            }
        }
    }
}