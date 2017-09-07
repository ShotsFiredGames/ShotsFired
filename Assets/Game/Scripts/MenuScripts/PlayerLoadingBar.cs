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
        if (photonView.OwnerShipWasTransfered)
        {
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