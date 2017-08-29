using UnityEngine;

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
        Local_SpawnFlag(viewID);
        PhotonView.RPC("RPC_SpawnFlag", PhotonTargets.Others, viewID);
        spawnedFlags = true;
    }

    void Local_SpawnFlag(int _viewID)
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

    [PunRPC]
    void RPC_SpawnFlag(int _viewID)
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
        {
            if (PhotonNetwork.isMasterClient)
            {
                InitFlags();
            }              
        }

        if (usedFlag != null)
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