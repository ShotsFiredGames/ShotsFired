using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EventManager : NetworkBehaviour
{
    public delegate void OnAddOnsAdded(List<AddOn> addOns);
    public static event OnAddOnsAdded OnAddOnAdded;

    public GameEvent[] allEvents;
    public static GameEvent currentEvent;
    GameEvent nextEvent;
    
    public List<AddOn> allAddOns;

    public static List<AddOn> AllAddOns
    {
        get;
        protected set;
    }

    public static AddOn currentAddOn;
    //AddOn nextAddOn;

    List<GameEvent> gameEvents = new List<GameEvent>();
    List<AddOn> addOns = new List<AddOn>();

    List<string> eventNames;
    List<string> addOnNames;

    void OnValidate()
    {
        Debug.Log("EM AddOn Added");
        AllAddOns = allAddOns;
        for (int i = 0; i < allEvents.Length; i++)
        {
            allEvents[i].allAddOns = allAddOns;
        }
        //OnAddOnAdded(allAddOns);
    }
    
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

        InvokeRepeating("ActivateNextEvent", GameCustomization.eventOccurenceRate, GameCustomization.eventOccurenceRate);
	}

    [ServerCallback]
    void ActivateNextEvent()
    {
        byte newEvent = (byte)Random.Range(0, gameEvents.Count);
        byte newAddOn = (byte)Random.Range(0, addOns.Count);
        RpcActivateNextEvent(newEvent, newAddOn);
    }

    [ClientRpc]
    void RpcActivateNextEvent(byte _newEvent, byte _newAddOn)
    {
        nextEvent = gameEvents[_newEvent];

        if (currentEvent != null && nextEvent.nameEvent.Equals(currentEvent.nameEvent))
            currentEvent.ResetEvent();
        else
        {
            currentEvent = nextEvent;
            currentEvent.StartEvent();
        }

        currentAddOn = addOns[_newAddOn];
        currentAddOn.StartAddOn();
    }
}
