using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EventManager : NetworkBehaviour
{
    public GameEvent[] allEvents;
    public static GameEvent currentEvent;

    List<GameEvent> gameEvents;

    List<string> eventNames;

    // Use this for initialization
    void Start ()
    {
        //Grabs a List of Usable Event Names From GameCustomization
        eventNames = GameCustomization.currentEvents;

        //Loops throught AllEvents and the List of Names
        foreach (GameEvent events in allEvents)
        {
            string currentName = events.nameEvent;

            foreach (string name in eventNames)
            {
                if (currentName.Equals(name))   //if the names are equal
                    gameEvents.Add(events);     //add the event to the playable list of events
            }
        }

        InvokeRepeating("ActivateNextEvent", 0, GameCustomization.eventOccurenceRate);
	}

    [ServerCallback]
    void ActivateNextEvent()
    {
        currentEvent = gameEvents[Random.Range(0, gameEvents.Count)];
        currentEvent.StartEvent();
    }
}
