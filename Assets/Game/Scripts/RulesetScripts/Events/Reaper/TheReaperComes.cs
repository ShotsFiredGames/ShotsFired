using UnityEngine;
using System.Collections.Generic;

public class TheReaperComes : GameEvent
{
    public GameObject[] objectsToSetActive;
    public GameObject reaper;
    public static List<Reaper> reapers = new List<Reaper>();
    public Transform[] reaperSpawns;
    [Tooltip("This number is subtracted. Make it positive if you want the player to lose points")]
    public byte pointsPlayerLosesOnDeath;

    void Awake()
    {
        PhotonView = GetComponent<PhotonView>();
    }

    private void InitReapers()
    {
        byte num = PlayerWrangler.GetNumOfPlayers();

        for (byte i = 0; i < num; i++)
        {
            if (PhotonNetwork.isMasterClient)
            {
                GameObject newReaper = PhotonNetwork.Instantiate(reaper.name, Vector3.zero, Quaternion.identity, 0);
                reapers.Add(newReaper.GetComponent<Reaper>());
            }           
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