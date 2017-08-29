using UnityEngine;
using UnityEngine.UI;

public class PlayerFlagInfo : MonoBehaviour
{
    public bool hasFlag { get; set; }
    public Image haveFlag;
    public Transform carriedFlagPosition;
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        haveFlag.gameObject.SetActive(hasFlag);
    }

    public void DropFlag()
    {
        if (hasFlag)
        {
            FlagManager.instance.Local_FlagDropped(name);
            FlagManager.instance.photonView.RPC("RPC_FlagDropped", PhotonTargets.All, name);
        }
    }

    public Vector3 GetCarriedFlagPostion()
    {
        return carriedFlagPosition.position;
    }
}
