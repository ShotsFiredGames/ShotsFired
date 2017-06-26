using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EventManager : NetworkBehaviour
{
    public GameEvent[] allEvents;
    public static GameEvent currentEvent;
    GameEvent nextEvent;

    public AddOn[] allAddOns;
    public static AddOn currentAddOn;
    //AddOn nextAddOn;

    List<GameEvent> gameEvents = new List<GameEvent>();
    List<AddOn> addOns = new List<AddOn>();

    List<string> eventNames;
    List<string> addOnNames;
    
    void Start ()
    {
        eventNames = GameCustomization.currentEvents;                       //Grabs a List of Usable Event Names From GameCustomization
        addOnNames = GameCustomization.currentAddOns;

                                                                            //Loops throught AllEvents and the List of Names
        foreach (GameEvent events in allEvents)
        {
            string currentName = events.nameEvent;

            foreach (string name in eventNames)
            {
                if (currentName.Equals(name))                               //if the names are equal
                    gameEvents.Add(events);                                 //add the event to the playable list of events
            }
        }

                                                                            //Loops throught AllAddOns and the List of Names
        foreach (AddOn addOn in allAddOns)
        {
            string currentName = addOn.addOnName;

            foreach (string name in addOnNames)
            {
                if (currentName.Equals(name))                               //if the names are equal
                    addOns.Add(addOn);                                      //add the event to the playable list of events
            }
        }

        InvokeRepeating("ActivateNextEvent", 10, GameCustomization.eventOccurenceRate);
	}

    [ServerCallback]
    void ActivateNextEvent()
    {
        byte newEvent = (byte)Random.Range(0, gameEvents.Count);
        RpcActivateNextEvent(newEvent);
    }

    [ClientRpc]
    void RpcActivateNextEvent(byte newEvent)
    {
        nextEvent = gameEvents[newEvent];

        if (currentEvent != null && nextEvent.nameEvent.Equals(currentEvent.nameEvent))
            currentEvent.ResetEvent();
        else
        {
            currentEvent = nextEvent;
            currentEvent.StartEvent();
        }

        currentAddOn = addOns[Random.Range(0, addOns.Count)];
        currentAddOn.StartAddOn();
    }
}
