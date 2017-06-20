using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Flag : NetworkBehaviour
{
    CaptureTheFlag captureTheFlag;
    GameObject carrier;
    [HideInInspector]
    public GameObject respawnPoint;
    Coroutine resetTimer;

    [ServerCallback]
    private void Awake()
    {
        NetworkServer.Spawn(gameObject);
    }

    private void Start()
    {
        captureTheFlag = GameObject.Find("CaptureTheFlag").GetComponent<CaptureTheFlag>();
    }

    public void SetSpawnPosition(GameObject spawn)
    {
        respawnPoint = spawn;
    }
    
    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Player"))
        {
            RpcFlagPickedUp(other.GetComponent<NetworkIdentity>());
        }
    }

    [ClientRpc]
    void RpcFlagPickedUp(NetworkIdentity player)
    {
        Debug.LogError("Picked Up teh flag");
        if (resetTimer != null)
            StopCoroutine(resetTimer);

        carrier = player.gameObject;
        carrier.GetComponent<PlayerManager>().hasFlag = true;
        gameObject.transform.SetParent(carrier.transform);
        gameObject.transform.position = carrier.transform.position + new Vector3(0, carrier.transform.localScale.y, 0);
    }

    [Command]
    public void CmdFlagReturned()
    {
        RpcFlagReturned();
    }

    [ClientRpc]
    public void RpcFlagReturned()
    {
        captureTheFlag.FlagReturned(carrier.name);
        CmdReturnFlag();
    }

    [Command]
    public void CmdFlagDropped()
    {
        RpcFlagDropped();
    }

    [ClientRpc]
    public void RpcFlagDropped()
    {
        transform.SetParent(null);
        resetTimer = StartCoroutine(captureTheFlag.ResetTimer());
    }

    [Command]
    public void CmdReturnFlag()
    {
        RpcReturnFlag();
    }

    [ClientRpc]
    void RpcReturnFlag()
    {
        transform.parent = respawnPoint.transform;
        transform.position = respawnPoint.transform.position + new Vector3(0, 2, 0);
    }
}
