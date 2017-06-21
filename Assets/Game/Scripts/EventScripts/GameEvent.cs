using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameEvent : MonoBehaviour {

    public float duration;
    public string nameEvent;

    public abstract void StartEvent();
    public abstract void EndEvent();

    Coroutine gameEventDur;

    /*void StartEvent()
     * // turn on all of what you need for the event
     * gameEventDur = StartCorountine(EventDuration());
     * */

    public void ResetEvent()
    {
        StopCoroutine(gameEventDur);
        gameEventDur = StartCoroutine(EventDuration());
    }

    public IEnumerator EventDuration()
    {
        yield return new WaitForSeconds(duration);
        EndEvent();
    }

}
