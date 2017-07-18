using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallToTheWall : GameEvent
{
    [HideInInspector]
    public bool ballToTheWallActive;
    public GameObject[] objectsToSetActive;
    public GameObject[] objectsToSetInActive;
    public GameObject[] spawnpoints;
    public Collider[] goals;

    public override void StartEvent()
    {
        ballToTheWallActive = true;
        for (int i = 0; i < objectsToSetInActive.Length; i++)
            objectsToSetInActive[i].SetActive(false);

        for (int i = 0; i < objectsToSetActive.Length; i++)
            objectsToSetActive[i].SetActive(true);

        for (int i = 0; i < PlayerWrangler.GetAllPlayers().Length; i++)
        {
            PlayerWrangler.GetAllPlayers()[i].transform.position = spawnpoints[i].transform.position;
            PlayerWrangler.GetAllPlayers()[i].transform.rotation = spawnpoints[i].transform.rotation;
        }

        for (int i = 0; i < goals.Length; i++)
            goals[i].enabled = true;

            StartCoroutine(EventDuration());

    }

    public override void EndEvent()
    {
        for (int i = 0; i < goals.Length; i++)
            goals[i].enabled = false;

        for (int i = 0; i < objectsToSetActive.Length; i++)
            objectsToSetActive[i].SetActive(false);

        for (int i = 0; i < objectsToSetInActive.Length; i++)
            objectsToSetInActive[i].SetActive(true);

        ballToTheWallActive = false;
    }

    new IEnumerator EventDuration()
    {
        yield return new WaitForSeconds(duration);
        EndEvent();
    }
}
