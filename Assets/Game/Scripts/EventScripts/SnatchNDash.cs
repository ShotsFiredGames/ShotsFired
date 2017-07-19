using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SnatchNDash : GameEvent
{
    public GameObject[] objectsToSetActive;
    public Flag flag;
    public FlagBase[] bases;
    public byte pointsForCapture;
    public float flagResetTime;

    [Space, Header("Audio Variables")]
    public AudioSource flagSource;
    public AudioClip pickupClip;
    public AudioClip dropClip;
    public AudioClip returnedClip;

    List<Flag> snashFlags = new List<Flag>();
    Coroutine captureTheFlag;

    bool spawnedFlags;

    void Start()
    {
        ClientScene.RegisterPrefab(flag.gameObject);
    }

    private void InitFlags()
    {
        PlayerManager[] players = PlayerWrangler.GetAllPlayers();

        for (byte i = 0; i < players.Length; i++)
        {
            Flag newFlag = Instantiate(flag);
            FlagManager.instance.flags.Add(newFlag);
            snashFlags.Add(newFlag);

            if (isServer)
            {
                NetworkServer.Spawn(newFlag.gameObject);
            }
            else
            {
                ClientScene.RegisterPrefab(newFlag.gameObject);
            }

            newFlag.index = FlagManager.instance.GetFlagNumber();
            newFlag.spawnPosition = bases[i].gameObject.transform;
            bases[i].flag = newFlag.gameObject;
            bases[i].owner = players[i];
            newFlag.flagBase = bases[i];

        }

        spawnedFlags = true;
    }

    public override void StartEvent()
    {
        foreach (GameObject go in objectsToSetActive)
            go.SetActive(false);

        foreach (FlagBase fb in bases)
            fb.gameObject.SetActive(true);

        if (!spawnedFlags)
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

