using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TheReaperComes : GameEvent
{
    public GameObject[] objectsToSetActive;
    public Reaper reaper;
    public Transform reaperSpawn;

    [ServerCallback]
    private void Start()
    {
        NetworkServer.Spawn(reaper.gameObject);
    }

    public override void StartEvent()
    {
        foreach (GameObject go in objectsToSetActive)
            go.SetActive(false);

        reaper.enabled = true;
        reaper.SetSpawnPoint(reaperSpawn);
        reaper.Setup();

        gameEventDur = StartCoroutine(EventDuration());
    }

    public override void EndEvent()
    {
        foreach (GameObject go in objectsToSetActive)
            go.SetActive(true);

        reaper.StopRespawn();
        reaper.enabled = false;
        reaper.gameObject.SetActive(false);
        EventManager.currentEvent = null;
    }
}
