using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Flag : MonoBehaviour
{
    [HideInInspector]
    public GameObject respawnPoint;

    CaptureTheFlag captureTheFlag;
    Coroutine resetTimer;
    GameObject carrier;

    private void Start()
    {
        captureTheFlag = GameObject.Find("CaptureTheFlag").GetComponent<CaptureTheFlag>();
    }

    public void SetSpawnPosition(GameObject spawn)
    {
        respawnPoint = spawn;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Player"))
        {
            captureTheFlag.CmdFlagPickedUp(other.GetComponent<NetworkIdentity>());
        }
    }
}
