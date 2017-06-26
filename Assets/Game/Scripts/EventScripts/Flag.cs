using UnityEngine;
using UnityEngine.Networking;

public class Flag : MonoBehaviour
{
    CaptureTheFlag captureTheFlag;
    Coroutine resetTimer;
    GameObject carrier;

    [HideInInspector]
    public bool isPickedUp;

    private void Start()
    {
        captureTheFlag = GameObject.Find("CaptureTheFlag").GetComponent<CaptureTheFlag>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Player") && !isPickedUp)
        {
            isPickedUp = true;
            captureTheFlag.CmdFlagPickedUp(other.GetComponent<NetworkIdentity>());
        }
    }
}
