using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class SnatchNDash : GameEvent
{
    public GameObject[] objectsToSetActive;
    public GameObject flag;
    public FlagBase[] bases;
    public byte pointsForCapture;
    public float flagResetTime;

    List<Flag> snashFlags = new List<Flag>();
    Coroutine captureTheFlag;
	List<PlayerManager> players = new List<PlayerManager>();

    bool spawnedFlags;

	void Awake()
	{
		PhotonView = GetComponent<PhotonView>();
	}

    private void InitFlags()
	{
		players = PlayerWrangler.GetAllPlayers().ToList();
		for (int i = 0; i < players.Count; i++)
        {
			int viewID = PhotonNetwork.AllocateViewID ();
			PhotonView.RPC ("RPC_SpawnFlags", PhotonTargets.All, viewID, i, players[i].name);
        }

        spawnedFlags = true;
    }

	[PunRPC]
	void RPC_SpawnFlags(int _viewID, int index, string _ownerID)
	{
		Flag newFlag = Instantiate(flag).GetComponent<Flag>();
		newFlag.GetComponent<PhotonView> ().viewID = _viewID;
		newFlag.flagBase = bases[index];
		FlagManager.instance.flags.Add(newFlag);
		snashFlags.Add(newFlag);

		newFlag.index = FlagManager.instance.GetFlagNumber();
		newFlag.spawnPosition = bases[index].gameObject.transform;
		newFlag.flagResetTime = flagResetTime;
		bases[index].flag = newFlag;
		PlayerManager _owner = PlayerWrangler.GetPlayer (_ownerID);
		bases[index].owner = _owner;
		newFlag.gameObject.SetActive(true);
	}

    public override void StartEvent()
    {
        foreach (GameObject go in objectsToSetActive)
            go.SetActive(false);

        foreach (FlagBase fb in bases)
            fb.gameObject.SetActive(true);

		if (PhotonNetwork.isMasterClient && !spawnedFlags)
            InitFlags();

        ActivateFlags(true);
        //flagSource = flag.GetComponent<AudioSource>();

        FlagManager.instance.pointsForCapture = pointsForCapture;
        gameEventDur = StartCoroutine(EventDuration());
    }

    public override void EndEvent()
    {
        foreach (GameObject go in objectsToSetActive)
            go.SetActive(true);

        foreach (FlagBase fb in bases)
            fb.gameObject.SetActive(false);

        ActivateFlags(false);

        EventManager.currentEvent = null;
    }

    void ActivateFlags(bool isActive)
    {
        for (byte i = 0; i < snashFlags.Count; i++)
        {
            snashFlags[i].enabled = isActive;
            snashFlags[i].gameObject.SetActive(isActive);
        }
    }
}