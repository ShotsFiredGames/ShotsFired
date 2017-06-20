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

    GameManager gameManager;
    Coroutine captureTheFlag;

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

    public void FlagDropped()
    {
        flag.GetComponent<Flag>().CmdFlagDropped();
    }

    public void FlagReturned(string player)
    {
        gameManager.FlagCaptured(player, pointsForCapture);
    }

    public IEnumerator ResetTimer()
    {
        yield return new WaitForSeconds(flagResetTime);
        flag.CmdReturnFlag();
    }
}
