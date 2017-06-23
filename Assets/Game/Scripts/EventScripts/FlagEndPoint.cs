using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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
