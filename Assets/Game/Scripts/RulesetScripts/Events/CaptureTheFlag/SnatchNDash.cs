using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

        foreach (FlagBase bas in bases)
        {
            bas.gameObject.SetActive(false);
        }
	}

    private void InitFlags()
	{
		players = PlayerWrangler.GetAllPlayers().ToList();
		for (int i = 0; i < players.Count; i++)
        {
			int viewID = PhotonNetwork.AllocateViewID ();
            Local_SpawnFlags(viewID, i, players[i].name);
			PhotonView.RPC ("RPC_SpawnFlags", PhotonTargets.Others, viewID, i, players[i].name);
        }

        spawnedFlags = true;
    }

    void Local_SpawnFlags(int _viewID, int index, string _ownerID)
    {
        Flag newFlag = Instantiate(flag).GetComponent<Flag>();
        newFlag.GetComponent<PhotonView>().viewID = _viewID;
        newFlag.flagBase = bases[index];
        FlagManager.instance.flags.Add(newFlag);
        snashFlags.Add(newFlag);

        newFlag.index = FlagManager.instance.GetFlagNumber();
        newFlag.spawnPosition = bases[index].gameObject;
        newFlag.flagResetTime = flagResetTime;
        bases[index].flag = newFlag;
        PlayerManager _owner = PlayerWrangler.GetPlayer(_ownerID);
        bases[index].owner = _owner.GetComponent<PlayerFlagInfo>();
        newFlag.gameObject.SetActive(true);
        bases[index].gameObject.SetActive(true);
        bases[index].gameObject.GetComponent<Renderer>().material.color = PlayerWrangler.GetFactionMaterial(_owner.faction).color;
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
		newFlag.spawnPosition = bases[index].gameObject;
		newFlag.flagResetTime = flagResetTime;
		bases[index].flag = newFlag;
		PlayerManager _owner = PlayerWrangler.GetPlayer (_ownerID);
		bases[index].owner = _owner.GetComponent<PlayerFlagInfo>();
		newFlag.gameObject.SetActive(true);
        bases[index].gameObject.SetActive(true);
        bases[index].gameObject.GetComponent<Renderer>().material.color = PlayerWrangler.GetFactionMaterial(_owner.faction).color;
    }

    public override void StartEvent()
    {
        foreach (GameObject go in objectsToSetActive)
            go.SetActive(false);

		if (PhotonNetwork.isMasterClient && !spawnedFlags)
            InitFlags();

        foreach (FlagBase fb in bases)
        {
            if (fb.owner != null)
                fb.gameObject.SetActive(true);
        }

        ActivateFlags(true);

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