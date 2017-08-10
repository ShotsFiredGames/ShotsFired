using UnityEngine;
using UnityEngine.UI;

public class PlayerLoadingBar : Photon.MonoBehaviour
{
    float progress;
    Image loadBar;

    private void Start()
    {
        loadBar = GetComponent<Image>();        
    }

    void Update()
    {
        //Debug.LogError(photonView.OwnerShipWasTransfered);
        if (photonView.OwnerShipWasTransfered)
        {
            //Debug.LogError(PhotonNetwork.player + " is local player. " + photonView.owner.NickName + " owns this photonview");
            //Debug.LogError(photonView.owner);
            if (photonView.isMine)
            {
                loadBar.fillAmount = ServerLauncher.instance.Progress;
            }
            else
            {
                loadBar.fillAmount = progress;
            }
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(ServerLauncher.instance.Progress);
        }
        else
        {
            progress = (float)stream.ReceiveNext();
        }
    }
}