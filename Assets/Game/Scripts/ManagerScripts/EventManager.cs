using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class EventManager : NetworkBehaviour
{
    public GameEvent[] allEvents;
    public static GameEvent currentEvent;
    GameEvent nextEvent;

    public AddOn[] allAddOns;
    public static AddOn currentAddOn;

    public bool isTesting;

    List<GameEvent> gameEvents = new List<GameEvent>();
    List<AddOn> addOns = new List<AddOn>();

    List<string> eventNames;
    List<string> addOnNames;

    void Start()
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

        InvokeRepeating("ActivateNextEvent", GameCustomization.eventOccurenceRate / 2, GameCustomization.eventOccurenceRate);
    }

    [ServerCallback]
    void ActivateNextEvent()
    {
        byte newEvent = (byte)GetRandomNumber(gameEvents.Count);
        RpcActivateNextEvent(newEvent);
    }

    [ClientRpc]
    void RpcActivateNextEvent(byte _newEvent)
    {
        nextEvent = gameEvents[_newEvent];

        if (currentEvent != null && nextEvent.nameEvent.Equals(currentEvent.nameEvent))
            currentEvent.ResetEvent();
        else
        {
            currentEvent = nextEvent;
            currentEvent.StartEvent();
        }

        StartCoroutine(NextAddons((byte)currentEvent.duration));
    }

    IEnumerator NextAddons(byte eventLength)
    {
        yield return new WaitForSeconds(eventLength / 3); //wait a third of the time
        ActivateNextAddon();
        yield return new WaitForSeconds(eventLength / 3); // wait another third of the time
        ActivateNextAddon();
    }

    [ServerCallback]
    void ActivateNextAddon()
    {
        byte newAddOn = (byte)GetRandomNumber(addOns.Count);
        RpcActiveNextAddon(newAddOn);
    }

    [ClientRpc]
    void RpcActiveNextAddon(byte _newAddOn)
    {
        currentAddOn = addOns[_newAddOn];
        currentAddOn.StartAddOn();
    }

    int GetRandomNumber(int max)
    {
        if (isTesting)
            return 0;
        else
            return Random.Range(0, max);
    }
}
