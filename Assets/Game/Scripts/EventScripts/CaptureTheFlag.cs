using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureTheFlag : GameEvent
{
    public GameObject[] objectsToSetActive;
    public GameObject flag;
    public GameObject flagSpawnpoint;
    public GameObject[] endPoints;
    public float eventLength;

    Coroutine captureTheFlag;

    public override void StartEvent()
    {
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
        yield return new WaitForSeconds(eventLength);
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
}
