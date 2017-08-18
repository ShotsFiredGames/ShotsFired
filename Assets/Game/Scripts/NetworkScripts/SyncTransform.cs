using UnityEngine;

public class SyncTransform : MonoBehaviour, IPunObservable
{
    Vector3 syncPos;
    Quaternion syncRot;
    [SerializeField]
    float lerpRate = 15;
	public PhotonView PhotonView { get; private set;}

	void Awake()
	{
		PhotonView = GetComponent<PhotonView> ();
	}

    void Update()
    {
        LerpPlayer();        
    }

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
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
		if (!PhotonView.isMine)
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
}