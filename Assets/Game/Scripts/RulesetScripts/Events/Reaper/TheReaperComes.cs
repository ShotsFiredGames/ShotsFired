using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class TheReaperComes : GameEvent
{
    public GameObject[] objectsToSetActive;
    public Reaper reaper;
    public static List<Reaper> reapers = new List<Reaper>();
    public Transform[] reaperSpawns;
    [Tooltip("This number is subtracted. Make it positive if you want the player to lose points")]
    public byte pointsPlayerLosesOnDeath;

    void Start()
    {
        ClientScene.RegisterPrefab(reaper.gameObject);
    }

    private void InitReapers()
    {
        byte num = PlayerWrangler.GetNumOfPlayers();

        for (byte i = 0; i < num; i++)
        {
            Reaper newReaper = Instantiate(reaper);
            reapers.Add(newReaper);

            if (isServer)
            {
                NetworkServer.Spawn(newReaper.gameObject);
            }
            else
            {
                ClientScene.RegisterPrefab(newReaper.gameObject);
            }
                
        }
    }

    public override void StartEvent()
    {
        foreach (GameObject go in objectsToSetActive)
        {
            if(go != null)
                go.SetActive(false);
        }
                
        if (reapers.Count < 1)
            InitReapers();

        PlayerManager[] players = PlayerWrangler.GetAllPlayers();

        for (byte index = 0; index < players.Length; index++)
        {
            reapers[index].enabled = true;
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
        {
            if(go != null)
                go.SetActive(true);
        }

        foreach (Reaper reaps in reapers)
        {
            reaps.StopReaper();
        }
        EventManager.currentEvent = null;
    }

    public static Reaper GetReaperChasingWhom(string playerChased)
    {
        Reaper persuer = null;

        foreach (Reaper reaps in reapers)
        {
            if (reaps.GetTargetPlayer().Equals(playerChased))
            {
                persuer = reaps;
                break;
            }
        }

        return persuer;
    }
}