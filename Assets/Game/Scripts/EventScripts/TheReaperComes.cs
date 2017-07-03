using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class TheReaperComes : GameEvent
{
    public GameObject[] objectsToSetActive;
    public Reaper reaper;
    List<Reaper> reapers = new List<Reaper>();
    public Transform[] reaperSpawns;
    [Tooltip("This number is subtracted. Make it positive if you want the player to lose points")]
    public byte pointsPlayerLosesOnDeath;

    [ServerCallback]
    private void Start()
    {
        
    }

    private void InitReapers()
    {
        byte num = PlayerWrangler.GetNumOfPlayers();

        for (byte i = 0; i < num; i++)
        {
            print("In here for" + i);
            reapers.Add(Instantiate(reaper));

            if (isServer)
                NetworkServer.Spawn(reapers[i].gameObject);
        }
    }

    public override void StartEvent()
    {
        foreach (GameObject go in objectsToSetActive)
            go.SetActive(false);

        if (reapers.Count < 1)
            InitReapers();

        PlayerManager[] players = PlayerWrangler.GetAllPlayers();

        for (byte index = 0; index < players.Length; index++)
        {
            reapers[index].enabled = true;
            reapers[index].canDie = true;
            reapers[index].SetTargetPlayer(players[index]);
            reapers[index].SetSpawnPoint(reaperSpawns[index]);
            reapers[index].SetPoints(pointsPlayerLosesOnDeath);
            reapers[index].Setup();
        }

        gameEventDur = StartCoroutine(EventDuration());
    }

    public override void EndEvent()
    {
        foreach (GameObject go in objectsToSetActive)
            go.SetActive(true);

        reaper.StopReaper();
        EventManager.currentEvent = null;
    }
}
