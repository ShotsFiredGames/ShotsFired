using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class CaptureTheFlag : GameEvent
{
    public GameObject[] objectsToSetActive;
    public Flag flag;
    public GameObject flagSpawnpoint;
    public GameObject[] endPoints;
    public float eventLength;
    public int pointsForCapture;
    public float flagResetTime;

    GameObject carrier;
    GameManager gameManager;
    Coroutine captureTheFlag;
    Coroutine resetTimer;

    private void OnEnable()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    [ServerCallback]
    private void Start()
    {
        NetworkServer.Spawn(flag.gameObject);
    }

    public override void StartEvent()
    {
        Debug.LogError("EventStarted");
        foreach(GameObject go in objectsToSetActive)
            go.SetActive(false);

        foreach (GameObject go in endPoints)
            go.SetActive(true);
        
        flag.transform.parent = flagSpawnpoint.transform;
        flag.transform.position = flagSpawnpoint.transform.position + new Vector3(0, 2, 0);
        flag.enabled = true;
        flag.gameObject.SetActive(true);

        captureTheFlag = StartCoroutine(EventLength());
    }

    IEnumerator EventLength()
    {
        yield return new WaitForSeconds(duration);
        EndEvent();
    }

    public override void EndEvent()
    {
        foreach (GameObject go in objectsToSetActive)
            go.SetActive(true);

        foreach (GameObject go in endPoints)
            go.SetActive(false);

        flag.transform.parent = flagSpawnpoint.transform;
        flag.transform.position = flagSpawnpoint.transform.position + new Vector3(0, 2, 0);

        flag.gameObject.SetActive(false);
        flag.isPickedUp = false;
        flag.enabled = false;

        if (carrier != null)
            carrier.GetComponent<PlayerManager>().hasFlag = false;

        Debug.LogError("Carrier: " + carrier + " abiltiy to pick up flag: " + carrier.GetComponent<PlayerManager>().hasFlag);
    }

    public void FlagReturned(string player)
    {
        gameManager.FlagCaptured(player, pointsForCapture);
    }

    public IEnumerator ResetTimer()
    {
        yield return new WaitForSeconds(flagResetTime);
        CmdReturnFlag();
    }

    [Command]
    public void CmdFlagPickedUp(NetworkIdentity player)
    {
        RpcFlagPickedUp(player);
    }

    [ClientRpc]
    void RpcFlagPickedUp(NetworkIdentity player)
    {
        Debug.LogError("Picked Up teh flag");
        if (resetTimer != null)
            StopCoroutine(resetTimer);

        carrier = player.gameObject;
        carrier.GetComponent<PlayerManager>().hasFlag = true;
        flag.transform.SetParent(carrier.transform);
        flag.transform.position = carrier.transform.position + new Vector3(0, carrier.transform.localScale.y, 0);
    }

    [Command]
    public void CmdReturnFlag()
    {
        RpcReturnFlag();
    }

    [ClientRpc]
    void RpcReturnFlag()
    {
        StartCoroutine(CanBePickedUp());
        flag.transform.parent = flagSpawnpoint.transform;
        flag.transform.position = flagSpawnpoint.transform.position + new Vector3(0, 2, 0);

        if (carrier != null)
            carrier.GetComponent<PlayerManager>().hasFlag = false;
    }

    [Command]
    public void CmdFlagDropped()
    {
        RpcFlagDropped();
    }

    [ClientRpc]
    public void RpcFlagDropped()
    {
        Debug.LogError("Flag Detached");
        if (carrier != null)
            carrier.GetComponent<PlayerManager>().hasFlag = false;

        flag.transform.parent = null;
        StartCoroutine(CanBePickedUp());
        resetTimer = StartCoroutine(ResetTimer());
    }

    [Command]
    public void CmdFlagReturned()
    {
        RpcFlagReturned();
    }

    [ClientRpc]
    public void RpcFlagReturned()
    {
        FlagReturned(carrier.name);
        CmdReturnFlag();
    }

    IEnumerator CanBePickedUp()
    {
        yield return new WaitForSeconds(2);
        flag.isPickedUp = false;
    }
}
