using UnityEngine;
using System.Collections.Generic;

public class TheReaperComes : GameEvent
{
    public GameObject[] objectsToSetActive;
    public GameObject reaper;
    public static List<Reaper> reapers = new List<Reaper>();
    public Transform reaperSpawn;
    [Tooltip("This number is subtracted. Make it positive if you want the player to lose points")]
    public byte pointsPlayerLosesOnDeath;

    void Awake()
    {
        PhotonView = GetComponent<PhotonView>();
    }

    private void InitReapers()
    {
        int viewID = PhotonNetwork.AllocateViewID();
        string target = GameManager.instance.GetWinningPlayer();
        Local_SpawnReapers(viewID, target);
        PhotonView.RPC("RPC_SpawnReapers", PhotonTargets.Others, viewID, target);
    }

    void Local_SpawnReapers(int _viewID, string targetID)
    {
        GameObject newReaper = Instantiate(reaper);
        Reaper _reaper = newReaper.GetComponent<Reaper>();
        newReaper.GetComponent<PhotonView>().viewID = _viewID;
        reapers.Add(_reaper);
        _reaper.enabled = true;
        _reaper.SetTargetPlayer(targetID);
        _reaper.SetSpawnPoint(reaperSpawn);
        _reaper.SetPoints(pointsPlayerLosesOnDeath);
        _reaper.Setup();
    }

    [PunRPC]
    void RPC_SpawnReapers(int _viewID, string targetID)
    {
        GameObject newReaper = Instantiate(reaper);
        Reaper _reaper = newReaper.GetComponent<Reaper>();
        newReaper.GetComponent<PhotonView>().viewID = _viewID;
        reapers.Add(_reaper);
        _reaper.enabled = true;
        _reaper.SetTargetPlayer(targetID);
        _reaper.SetSpawnPoint(reaperSpawn);
        _reaper.SetPoints(pointsPlayerLosesOnDeath);
        _reaper.Setup();
    }

    public override void StartEvent()
    {
        foreach (GameObject go in objectsToSetActive)
            go.SetActive(false);

        if (PhotonNetwork.isMasterClient && reapers.Count < 1)
            InitReapers();

        byte num = PlayerWrangler.GetNumOfPlayers();

        if (reapers.Count > 0)
        {
            for (byte index = 0; index < num; index++)
            {
                reapers[index].enabled = true;
                reapers[index].Setup();
            }
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