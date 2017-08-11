using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class EventManager : Photon.MonoBehaviour
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

    private void Awake()
    {
        PhotonNetwork.OnEventCall += ActivateEvents;
    }

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
    }

    void ActivateEvents(byte eventcode, object content, int senderid)
    {
        if (eventcode == 0 && PhotonNetwork.isMasterClient)
            InvokeRepeating("ActivateNextEvent", GameCustomization.eventOccurenceRate / 2, GameCustomization.eventOccurenceRate);
    }

    void ActivateNextEvent()
    {
        byte newEvent = (byte)Random.Range(0, gameEvents.Count);
        photonView.RPC("RPC_ActivateNextEvent", PhotonTargets.All, newEvent);
    }

    [PunRPC]
    void RPC_ActivateNextEvent(byte _newEvent)
    {
        nextEvent = gameEvents[_newEvent];

        if (currentEvent != null && nextEvent.nameEvent.Equals(currentEvent.nameEvent))
            currentEvent.ResetEvent();
        else
        {
            currentEvent = nextEvent;
            currentEvent.StartEvent();
            if(PhotonNetwork.isMasterClient)
            {
                int arrayIndex = AnnouncerManager.instance.GetRandomEventIndex(currentEvent.nameEvent);
                AnnouncerManager.instance.PhotonView.RPC("RPC_PlayEventStartClip", PhotonTargets.All, currentEvent.nameEvent, arrayIndex);
            }
        }

        if (PhotonNetwork.isMasterClient)
            StartCoroutine(NextAddons((byte)currentEvent.duration));
    }

    IEnumerator NextAddons(byte eventLength)
    {
        yield return new WaitForSeconds(eventLength / 3); //wait a third of the time
        ActivateNextAddon();
        yield return new WaitForSeconds(eventLength / 3); // wait another third of the time
        ActivateNextAddon();
    }

    void ActivateNextAddon()
    {
        byte newAddOn = (byte)Random.Range(0, addOns.Count);
        photonView.RPC("RPC_ActivateNextAddOn", PhotonTargets.All, newAddOn);
    }

    [PunRPC]
    void RPC_ActivateNextAddOn(byte _newAddOn)
    {
        currentAddOn = addOns[_newAddOn];
        currentAddOn.StartAddOn();

        if (PhotonNetwork.isMasterClient)
        {
            int arrayIndex = AnnouncerManager.instance.GetRandomAddOnIndex(currentAddOn.addOnName);
            AnnouncerManager.instance.PhotonView.RPC("RPC_PlayAddOnStartClip", PhotonTargets.All, currentAddOn.addOnName, arrayIndex);
        }
    }
}
