using System.Collections;
using UnityEngine;

public class DestroyObject : Photon.MonoBehaviour
{
    public float destroyAfter;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(destroyAfter);
        if (photonView.isMine)
            PhotonNetwork.Destroy(photonView);
    }
}