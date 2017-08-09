using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FirmlyGraspIt : GameEvent
{
    public GameObject[] objectsToSetActive;
    public CarrierFlag flagPrefab;
    public GameObject spawn;
    public byte pointsForHolding;
    public float timeIncrement;
    public float flagResetTime;

    CarrierFlag usedFlag;
    Coroutine captureTheFlag;

    bool spawnedFlags;

    void Awake()
    {
        PhotonView = GetComponent<PhotonView>();
    }

    private void InitFlags()
    {
        int viewID = PhotonNetwork.AllocateViewID();
        PhotonView.RPC("RPC_SpawnFlags", PhotonTargets.All, viewID);

        spawnedFlags = true;
    }

    [PunRPC]
    void RPC_SpawnFlags(int _viewID)
    {
        CarrierFlag newFlag = Instantiate(flagPrefab).GetComponent<CarrierFlag>();
        newFlag.GetComponent<PhotonView>().viewID = _viewID;
        FlagManager.instance.flags.Add(newFlag);

        newFlag.index = FlagManager.instance.GetFlagNumber();
        newFlag.spawnPosition = spawn;
        newFlag.flagResetTime = flagResetTime;
        newFlag.timeIncrement = timeIncrement;
        newFlag.gameObject.SetActive(true);
        usedFlag = newFlag;
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