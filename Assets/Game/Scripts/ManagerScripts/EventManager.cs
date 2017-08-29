﻿using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class EventManager : Photon.MonoBehaviour
{
    public GameEvent[] allEvents;
    public static GameEvent currentEvent;
    GameEvent nextEvent;

    public AddOn[] allAddOns;
    public static AddOn currentAddOn;
    AddOn nextAddOn;

    List<GameEvent> gameEvents = new List<GameEvent>();
    List<AddOn> addOns = new List<AddOn>();

    string eventNames;
    string addOnNames;

    void Start()
    {
        eventNames = GameCustomization.currentEvents;                       //Grabs a List of Wanted Events From GameCustomization
        addOnNames = GameCustomization.currentAddOns;

        for (int parse = 0; parse < eventNames.Length; parse++)
        {
            string charName = eventNames.Substring(parse, 1);

            if (charName.Equals("1"))
                gameEvents.Add(allEvents[parse]);
        }

        foreach (GameEvent ge in gameEvents)
        {
            Debug.LogError(ge.nameEvent + " is an activated Event");
        }

        for (int parse = 0; parse < addOnNames.Length; parse++)
        {
            string charName = addOnNames.Substring(parse, 1);

            if (charName.Equals("1"))
                addOns.Add(allAddOns[parse]);
        }

        foreach (AddOn ao in addOns)
        {
            Debug.LogError(ao.addOnName + " is an activated addon");
        }
    }

    void OnEnable()
    {
        PhotonNetwork.OnEventCall += StartEvents;
    }

    void OnDisable()
    {
        PhotonNetwork.OnEventCall -= StartEvents;
    }

    void StartEvents(byte eventcode, object content, int senderid)
    {
        if (eventcode == 0)
        {
            if (PhotonNetwork.isMasterClient)
                InvokeRepeating("ChooseNextEvent", GameCustomization.eventOccurenceRate / 2, GameCustomization.eventOccurenceRate);
        }
    }

    void ChooseNextEvent()
    {
        byte newEvent = 255;
        if (gameEvents.Count > 0)
            newEvent = (byte)Random.Range(0, gameEvents.Count);

        Local_ActivateNextEvent(newEvent);
        photonView.RPC("RPC_ActivateNextEvent", PhotonTargets.Others, newEvent);
    }

    void Local_ActivateNextEvent(byte _newEvent)
    {
        if (addOns.Count > 1)
        {
            if (currentAddOn != null)
            {
                currentAddOn.EndAddOn();
                currentAddOn = null;
            }
        }

        if (_newEvent != 255)
        {
            nextEvent = gameEvents[_newEvent];

            if (currentEvent != null && nextEvent.nameEvent.Equals(currentEvent.nameEvent))
                currentEvent.ResetEvent();
            else
            {
                currentEvent = nextEvent;
                currentEvent.StartEvent();
                if (PhotonNetwork.isMasterClient)
                {
                    int arrayIndex = AnnouncerManager.instance.GetRandomEventIndex(currentEvent.nameEvent);
                    AnnouncerManager.instance.Local_PlayEventStartClip(currentEvent.nameEvent, arrayIndex);
                    AnnouncerManager.instance.PhotonView.RPC("RPC_PlayEventStartClip", PhotonTargets.Others, currentEvent.nameEvent, arrayIndex);
                }
            }
        }
        else
            currentEvent = null;

        if (PhotonNetwork.isMasterClient && addOns.Count > 0)
            StartCoroutine(NextAddons(GetEventDuration(currentEvent)));
    }

    [PunRPC]
    void RPC_ActivateNextEvent(byte _newEvent)
    {
        if (addOns.Count > 1)
        {
            if (currentAddOn != null)
            {
                currentAddOn.EndAddOn();
                currentAddOn = null;
            }            
        }

        if (_newEvent != 255)
        {
            nextEvent = gameEvents[_newEvent];

            if (currentEvent != null && nextEvent.nameEvent.Equals(currentEvent.nameEvent))
                currentEvent.ResetEvent();
            else
            {
                currentEvent = nextEvent;
                currentEvent.StartEvent();
            }
        }
        else
            currentEvent = null;
    }

    byte GetEventDuration(GameEvent _currentEvent)
    {
        if (_currentEvent == null)
            return (byte)GameCustomization.eventOccurenceRate;
        else
            return (byte)_currentEvent.duration;
    }

    IEnumerator NextAddons(byte eventLength)
    {
        yield return new WaitForSeconds(eventLength / 3); //wait a third of the time
        ChooseNextAddon();
        yield return new WaitForSeconds(eventLength / 3); // wait another third of the time
        ChooseNextAddon();
    }

    void ChooseNextAddon()
    {
        byte newAddOn = 255;
        if (addOns.Count > 0)
            newAddOn = (byte)Random.Range(0, addOns.Count);

        Local_ActivateNextAddOn(newAddOn);
        photonView.RPC("RPC_ActivateNextAddOn", PhotonTargets.Others, newAddOn);
    }

    void Local_ActivateNextAddOn(byte _newAddOn)
    {
        if (_newAddOn != 255)
        {
            currentAddOn = addOns[_newAddOn];
            currentAddOn.StartAddOn();

            if (PhotonNetwork.isMasterClient)
            {
                int arrayIndex = AnnouncerManager.instance.GetRandomAddOnIndex(currentAddOn.addOnName);
                AnnouncerManager.instance.Local_PlayAddOnStartClip(currentAddOn.addOnName, arrayIndex);
                AnnouncerManager.instance.PhotonView.RPC("RPC_PlayAddOnStartClip", PhotonTargets.Others, currentAddOn.addOnName, arrayIndex);
            }
        }
    }

    [PunRPC]
    void RPC_ActivateNextAddOn(byte _newAddOn)
    {
        if (_newAddOn != 255)
        {
            nextAddOn = addOns[_newAddOn];

            if (currentAddOn != null)
            {
                if (currentAddOn.addOnName.Equals(nextAddOn.addOnName))
                    return;
                else
                    currentAddOn.EndAddOn();
            }

            currentAddOn = nextAddOn;
            currentAddOn.StartAddOn();
        }
    }
}