using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FlagEndPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Flag"))
        {
            other.GetComponent<Flag>().CmdFlagReturned();
        }
    }
}
