using UnityEngine;
using UnityEngine.UI;

public class PlayerSyncTransform : Photon.MonoBehaviour
{
    Vector3 syncPos;
    Quaternion syncRot;
    [SerializeField]
    float lerpRate = 15;

    #region Latency Variables
    private int latency;
    [SerializeField]
    Text latencyText;
    #endregion

    void Update()
    {
        LerpPlayer();
        ShowLatency();
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            syncPos = (Vector3)stream.ReceiveNext();
            syncRot = (Quaternion)stream.ReceiveNext();
        }
    }

    void LerpPlayer()
    {
        if (!photonView.isMine)
        {
            transform.position = Vector3.Lerp(transform.position, syncPos, Time.deltaTime * lerpRate);
            transform.rotation = Quaternion.Lerp(transform.rotation, syncRot, Time.deltaTime * lerpRate);
        }
    }

    void LerpPlayerRot(float newAngle)
    {
        Vector3 newRot = new Vector3(0, newAngle, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(newRot), Time.deltaTime * lerpRate);
    }

    void ShowLatency()
    {
        if (photonView.isMine)
        {
            latency = PhotonNetwork.GetPing();
            latencyText.text = latency.ToString();
        }
    }
}