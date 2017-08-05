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
		PlayerManager[] players = PlayerWrangler.GetAllPlayers();

		for (byte i = 0; i < players.Length; i++)
        {
			int viewID = PhotonNetwork.AllocateViewID ();
			PhotonView.RPC ("RPC_SpawnReapers", PhotonTargets.All, viewID, (byte) i, players[i].name);
        }
    }

	[PunRPC]
	void RPC_SpawnReapers(int _viewID, byte index, string targetID)
	{
		GameObject newReaper = Instantiate (reaper);
		Reaper _reaper = newReaper.GetComponent<Reaper> ();
		newReaper.GetComponent<PhotonView> ().viewID = _viewID;
		reapers.Add(_reaper);
		_reaper.enabled = true;
		_reaper.SetTargetPlayer (PlayerWrangler.GetPlayer (targetID));
		_reaper.SetSpawnPoint (reaperSpawns [index]);
		_reaper.SetPoints (pointsPlayerLosesOnDeath);
		_reaper.Setup ();

	}

    public override void StartEvent()
    {
        foreach (GameObject go in objectsToSetActive)
            go.SetActive(false);

		if (PhotonNetwork.isMasterClient && reapers.Count < 1)
            InitReapers();

		byte num = PlayerWrangler.GetNumOfPlayers();

		if (reapers.Count > 0) {
			for (byte index = 0; index < num; index++) {
				reapers [index].enabled = true;
				reapers [index].Setup ();
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