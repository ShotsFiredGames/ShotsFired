using UnityEngine;
using UnityEngine.Networking;

public class TheReaperComes : GameEvent
{
    public GameObject[] objectsToSetActive;
    public Reaper reaper;
    public Transform reaperSpawn;
    [Tooltip("This number is subtracted. Make it positive if you want the player to lose points")]
    public byte pointsPlayerLosesOnDeath;

    [ServerCallback]
    private void Start()
    {
        NetworkServer.Spawn(reaper.gameObject);
    }

    public override void StartEvent()
    {
        foreach (GameObject go in objectsToSetActive)
            go.SetActive(false);

        reaper.enabled = true;
        reaper.SetSpawnPoint(reaperSpawn);
        reaper.SetPoints(pointsPlayerLosesOnDeath);
        reaper.Setup();

        gameEventDur = StartCoroutine(EventDuration());
    }

    public override void EndEvent()
    {
        foreach (GameObject go in objectsToSetActive)
            go.SetActive(true);

        reaper.StopRespawn();
        reaper.enabled = false;
        reaper.gameObject.SetActive(false);
        EventManager.currentEvent = null;
    }
}
