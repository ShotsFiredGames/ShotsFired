//using System.Collections;
using ExitGames.Client.Photon;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizationToServer : MonoBehaviour
{
    public Button customizeMatchButton;

    [Header("Stats To Transfer")]
    public Slider playerHealth;
    public Slider respawnTime;
    public Slider playerSpeed;
    public Slider abilityDuration;
    public Slider eventOccurence;
    public Slider gameLength;
    public Slider pointsToWin;
    public Slider pointsPerKill;
    public Toggle unlimitedAmmo;

    [Header("Event Toggles")]
    public List<TextToggle> events = new List<TextToggle>();

    [Header("AddOn Toggles")]
    public List<TextToggle> addOns = new List<TextToggle>();

    Hashtable roomProps;

    public const string PLAYERHEALTH = "hp";
    public const string RESPAWNTIME = "rt";
    public const string PLAYERSPEED = "ps";
    public const string ABILITYDURATION = "ad";
    public const string EVENTOCCURENCE = "eo";
    public const string GAMELENGTH = "gl";
    public const string POINTSTOWIN = "pw";
    public const string POINTSTPERKILL = "pk";
    public const string UNLIMITEDAMMO = "ua";
    public const string EVENTSLISTED = "el";
    public const string ADDONSLISTED = "al";

    // Use this for initialization
    public void InitValues()
    {
        if (PhotonNetwork.room == null) return;
        if (roomProps != null) return;

        Debug.LogError("Init values");
        string eventsString = "";

        for (int i = 0; i < events.Count; i++)
        {
            eventsString += "1";
        }

        Debug.LogError("The events string is: " + eventsString);
        string addonString = "";

        for (int i = 0; i < addOns.Count; i++)
        {
            addonString += "1";
        }

        roomProps = new Hashtable() { { PLAYERHEALTH, playerHealth.value }, { RESPAWNTIME, respawnTime.value} , { PLAYERSPEED, playerSpeed.value},
            {ABILITYDURATION, abilityDuration.value }, { EVENTOCCURENCE, eventOccurence.value}, { GAMELENGTH, gameLength.value}, { POINTSTOWIN, pointsToWin.value},
            { POINTSTPERKILL, pointsPerKill.value}, { UNLIMITEDAMMO, unlimitedAmmo.isOn} , { EVENTSLISTED, eventsString}, { ADDONSLISTED, addonString} };
        PhotonNetwork.room.SetCustomProperties(roomProps);
    }

    public void UpdateValues()
    {
        roomProps[PLAYERHEALTH] = playerHealth.value;
        roomProps[RESPAWNTIME] = respawnTime.value;
        roomProps[PLAYERSPEED] = playerSpeed.value;
        roomProps[ABILITYDURATION] = abilityDuration.value;
        roomProps[EVENTOCCURENCE] = eventOccurence.value;
        roomProps[GAMELENGTH] = gameLength.value;
        roomProps[POINTSTOWIN] = pointsToWin.value;
        roomProps[POINTSTPERKILL] = pointsPerKill.value;
        roomProps[UNLIMITEDAMMO] = unlimitedAmmo.isOn;
        string eventsString = ConvertTextTogglesToString(events);
        string addonString = ConvertTextTogglesToString(addOns);
        roomProps[EVENTSLISTED] = eventsString;
        roomProps[ADDONSLISTED] = addonString;
        PhotonNetwork.room.SetCustomProperties(roomProps);
    }

    string ConvertTextTogglesToString(List<TextToggle> toggles)
    {
        string result = "";
        foreach (TextToggle tog in toggles)
        {
            if (tog.isActivate)
                result += "1";
            else
                result += "0";
        }

        return result;
    }
}