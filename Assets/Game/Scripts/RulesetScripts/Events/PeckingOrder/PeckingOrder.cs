using UnityEngine;

public class PeckingOrder : GameEvent
{
    public ControlPoint[] controlPoints;
    public GameObject[] objectsToSetActive;
    public short pointsRecieved;
    public double pointFreq;

    void Awake()
    {
        ActivateCPs(false);
    }

    public override void StartEvent()
    {
        foreach (ControlPoint cp in controlPoints)
        {
            cp.SetValues(pointsRecieved, pointFreq);
        }

        if (PlayerWrangler.GetNumOfPlayers() < 3)
        {
            controlPoints[0].gameObject.SetActive(true);
        }
        else
        {
            ActivateCPs(true);
        }

        gameEventDur = StartCoroutine(EventDuration());
    }

    public override void EndEvent()
    {
        ActivateCPs(false);

        EventManager.currentEvent = null;
    }

    void ActivateCPs(bool isActive)
    {
        foreach (ControlPoint cp in controlPoints)
        {
            cp.gameObject.SetActive(isActive);
        }
    }
}
