using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class GameEvent : NetworkBehaviour {

    public float duration;
    public string nameEvent;
    public List<AddOn> allAddOns;

    public abstract void StartEvent();
    public abstract void EndEvent();

    [HideInInspector]
    public Coroutine gameEventDur;

    /*void StartEvent()
     * // turn on all of what you need for the event
     * gameEventDur = StartCorountine(EventDuration());
     * */

    public void ResetEvent()
    {
        if(gameEventDur != null)
        StopCoroutine(gameEventDur);

        gameEventDur = StartCoroutine(EventDuration());
    }

    public IEnumerator EventDuration()
    {
        yield return new WaitForSeconds(duration);
        EndEvent();
    }

}