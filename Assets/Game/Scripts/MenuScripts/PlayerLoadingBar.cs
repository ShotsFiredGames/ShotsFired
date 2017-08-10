using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLoadingBar : MonoBehaviour
{
    float progress;
    Image loadBar;

    private void Start()
    {
        loadBar = GetComponent<Image>();
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.isWriting)
        {
            stream.SendNext(ServerLauncher.instance.Progress);
            loadBar.fillAmount = progress;
        }
        else
        {
            progress = (float)stream.ReceiveNext();
            loadBar.fillAmount = progress;
        }
    }
}
