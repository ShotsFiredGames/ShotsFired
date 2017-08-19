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
        CinematicCameraSystem.OnCinematicFinished += StartEvents;
    }

    void OnDisable()
    {
        CinematicCameraSystem.OnCinematicFinished -= StartEvents;
    }

    void StartEvents()
    {
        if (PhotonNetwork.isMasterClient)
            InvokeRepeating("ActivateNextEvent", GameCustomization.eventOccurenceRate / 2, GameCustomization.eventOccurenceRate);
    }



    void ActivateNextEvent()
    {
        byte newEvent = 255;
        if (gameEvents.Count > 0)
            newEvent = (byte)Random.Range(0, gameEvents.Count);

        photonView.RPC("RPC_ActivateNextEvent", PhotonTargets.All, newEvent);
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
                if (PhotonNetwork.isMasterClient)
                {
                    int arrayIndex = AnnouncerManager.instance.GetRandomEventIndex(currentEvent.nameEvent);
                    AnnouncerManager.instance.PhotonView.RPC("RPC_PlayEventStartClip", PhotonTargets.All, currentEvent.nameEvent, arrayIndex);
                }
            }
        }
        else
            currentEvent = null;

        if (PhotonNetwork.isMasterClient && addOns.Count > 0)
            StartCoroutine(NextAddons(GetEventDuration(currentEvent)));
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
        ActivateNextAddon();
        yield return new WaitForSeconds(eventLength / 3); // wait another third of the time
        ActivateNextAddon();
    }

    void ActivateNextAddon()
    {
        byte newAddOn = 255;
        if (addOns.Count > 0)
            newAddOn = (byte)Random.Range(0, addOns.Count);
        photonView.RPC("RPC_ActivateNextAddOn", PhotonTargets.All, newAddOn);
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

            if (PhotonNetwork.isMasterClient)
            {
                int arrayIndex = AnnouncerManager.instance.GetRandomAddOnIndex(currentAddOn.addOnName);
                AnnouncerManager.instance.PhotonView.RPC("RPC_PlayAddOnStartClip", PhotonTargets.All, currentAddOn.addOnName, arrayIndex);
            }
        }
    }
}