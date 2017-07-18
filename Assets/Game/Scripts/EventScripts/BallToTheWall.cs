using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BallToTheWall : GameEvent
{
    public short scoreAmount;

    [HideInInspector]
    public bool ballToTheWallActive;
    public GameObject[] objectsToSetActive;
    public GameObject[] objectsToSetInActive;
    public GameObject[] spawnpoints;
    public Collider[] goals;

    public GameObject ball;
    public GameObject ballRespawn;
    GameObject activeBall;

    [ServerCallback]
    private void Start()
    {
        activeBall = Instantiate(ball, ballRespawn.transform.position, ballRespawn.transform.rotation);
        activeBall.transform.parent = transform;
        activeBall.GetComponent<Ball>().SetVariables(this);
        NetworkServer.Spawn(activeBall);
        activeBall.SetActive(false);
    }

    public override void StartEvent()
    {
        ballToTheWallActive = true;
        activeBall.SetActive(true);

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

        activeBall.SetActive(false);
        ballToTheWallActive = false;
    }

    new IEnumerator EventDuration()
    {
        yield return new WaitForSeconds(duration);
        EndEvent();
    }

    public void PlayerScored(string player)
    {
        GameManager.instance.CmdAddScore(player, scoreAmount);
    }

    public void RespawnBall()
    {
        activeBall.transform.position = ballRespawn.transform.position;
    }
}
