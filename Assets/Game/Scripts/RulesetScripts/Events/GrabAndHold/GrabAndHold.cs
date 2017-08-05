using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GrabAndHold : GameEvent {

    public GameObject[] objectsToSetActive;
    public CarrierFlag flagPrefab;
    public GameObject spawn;
    public byte pointsForHolding;
    public float timeIncrement;
    public float flagResetTime;

    CarrierFlag usedFlag;
    Coroutine captureTheFlag;

    bool spawnedFlags;

    void Start()
    {
        ClientScene.RegisterPrefab(flagPrefab.gameObject);
    }

    private void InitFlags()
    {
        CarrierFlag newFlag = Instantiate(flagPrefab);
        FlagManager.instance.flags.Add(newFlag);

        if (isServer)
        {
            NetworkServer.Spawn(newFlag.gameObject);
        }
        else
        {
            ClientScene.RegisterPrefab(newFlag.gameObject);
        }

        newFlag.index = FlagManager.instance.GetFlagNumber();
        newFlag.spawnPosition = spawn.transform;
        newFlag.flagResetTime = flagResetTime;
        newFlag.timeIncrement = timeIncrement;
        newFlag.gameObject.SetActive(true);
        usedFlag = newFlag;

        spawnedFlags = true;
    }

    public override void StartEvent()
    {
        foreach (GameObject go in objectsToSetActive)
            go.SetActive(false);

        spawn.SetActive(true);

        if (!spawnedFlags)
            InitFlags();

        usedFlag.gameObject.SetActive(true);

        FlagManager.instance.pointsForHolding = pointsForHolding;
        gameEventDur = StartCoroutine(EventDuration());
    }

    public override void EndEvent()
    {
        foreach (GameObject go in objectsToSetActive)
            go.SetActive(true);

        spawn.SetActive(false);
        usedFlag.gameObject.SetActive(false);

        EventManager.currentEvent = null;
    }
}
