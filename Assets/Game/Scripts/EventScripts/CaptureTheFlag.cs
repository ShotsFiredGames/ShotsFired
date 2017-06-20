using System;
using System.Collections;
using UnityEngine;

public class CaptureTheFlag : GameEvent
{
    public GameObject[] objectsToSetActive;
    public GameObject flag;
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
    }

    public override void StartEvent()
    {
        Debug.LogError("EventStarted");
        foreach(GameObject go in objectsToSetActive)
            go.SetActive(false);

        foreach (GameObject go in endPoints)
            go.SetActive(true);

        flag.GetComponent<Flag>().enabled = true;
        flag.SetActive(true);

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

        flag.GetComponent<Flag>().enabled = false;
        flag.SetActive(false);
    }

    public void FlagDropped()
    {
        flag.GetComponent<Flag>().FlagDropped();
    }

    public void FlagReturned(string player)
    {
        gameManager.FlagCaptured(player, pointsForCapture);
        ReturnFlag();
    }

    void ReturnFlag()
    {
        flag.transform.position = flagSpawnpoint.transform.position + new Vector3(0, 2, 0);
    }

    public IEnumerator ResetTimer()
    {
        yield return new WaitForSeconds(flagResetTime);
        ReturnFlag();
    }
}
