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
        flag.SetSpawnPosition(flagSpawnpoint);
    }

    public override void StartEvent()
    {
        Debug.LogError("EventStarted");
        foreach(GameObject go in objectsToSetActive)
            go.SetActive(false);

        foreach (GameObject go in endPoints)
            go.SetActive(true);

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

        flag.transform.SetParent(flagSpawnpoint.transform);

        flag.gameObject.SetActive(false);
        flag.enabled = false;
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
        flag.transform.parent = flagSpawnpoint.transform;
        flag.transform.position = flagSpawnpoint.transform.position + new Vector3(0, 2, 0);
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
        flag.transform.SetParent(flag.transform);
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
}
