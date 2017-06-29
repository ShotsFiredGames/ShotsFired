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
    public byte pointsForCapture;
    public float flagResetTime;

    [Space, Header("Audio Variables")]
    public AudioSource flagSource;
    public AudioClip pickupClip;
    public AudioClip dropClip;
    public AudioClip returnedClip;

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
        foreach(GameObject go in objectsToSetActive)
            go.SetActive(false);

        foreach (GameObject go in endPoints)
            go.SetActive(true);
        
        flag.transform.parent = flagSpawnpoint.transform;
        flag.transform.position = flagSpawnpoint.transform.position + new Vector3(0, 2, 0);
        flag.enabled = true;
        flag.gameObject.SetActive(true);
        flagSource = flag.GetComponent<AudioSource>();

        gameEventDur = StartCoroutine(EventDuration());
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

        EventManager.currentEvent = null;
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
        if (resetTimer != null)
            StopCoroutine(resetTimer);

        carrier = player.gameObject;
        carrier.GetComponent<PlayerManager>().hasFlag = true;
        flag.transform.SetParent(carrier.transform);
        flag.transform.position = carrier.transform.position + new Vector3(0, carrier.transform.localScale.y, 0);
        if (flagSource != null)
            flagSource.PlayOneShot(pickupClip);
    }

    [Command]
    public void CmdReturnFlag()
    {
        RpcReturnFlag();
    }

    [ClientRpc]
    void RpcReturnFlag()
    {
        if (flagSource != null)
            flagSource.PlayOneShot(returnedClip);
        FlagReturned(carrier.name);
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
        if (carrier != null)
            carrier.GetComponent<PlayerManager>().hasFlag = false;

        flag.transform.parent = null;
        StartCoroutine(CanBePickedUp());
        resetTimer = StartCoroutine(ResetTimer());
        if(flagSource != null)
            flagSource.PlayOneShot(dropClip);
    }

    IEnumerator CanBePickedUp()
    {
        yield return new WaitForSeconds(2);
        flag.isPickedUp = false;
    }
}
