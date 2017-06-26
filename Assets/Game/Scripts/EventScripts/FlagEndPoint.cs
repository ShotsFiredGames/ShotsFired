using UnityEngine;

public class FlagEndPoint : MonoBehaviour
{
    public CaptureTheFlag captureTheFlag;

    private void Start()
    {
        captureTheFlag = GameObject.Find("CaptureTheFlag").GetComponent<CaptureTheFlag>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Flag"))
        {
           captureTheFlag.CmdFlagReturned();
        }
    }
}
