using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EventManager : NetworkBehaviour
{
    GameEvent[] events;

    // Use this for initialization
    void Start ()
    {
        events = GameCustomization.currentEvents.ToArray();
        InvokeRepeating("ActivateNextEvent", 0, GameCustomization.eventOccurenceRate);
	}

    [ServerCallback]
    void ActivateNextEvent()
    {
        events[Random.Range(0, events.Length)].StartEvent();
    }
}
