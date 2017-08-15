using UnityEngine;
using System.Collections.Generic;

public class GameCustomization : MonoBehaviour
{
    public static string currentEvents;
    public static string currentAddOns;
    public static float respawnTime;
    public static float playerSpeed;
    public static short playerHealth;
    public static bool isAmmoUnlimited;
    public static float abilityDuration;
    public static float eventOccurenceRate;
    public static byte gameLength;
    public static short pointsToWin;
    public static short pointsPerKill;

    private void Awake()
    {
        string hp = PhotonNetwork.room.CustomProperties[CustomizationToServer.PLAYERHEALTH].ToString();
        playerHealth = short.Parse(hp);
        string respawn = PhotonNetwork.room.CustomProperties[CustomizationToServer.RESPAWNTIME].ToString();
        respawnTime = float.Parse(respawn);
        string speed = PhotonNetwork.room.CustomProperties[CustomizationToServer.PLAYERSPEED].ToString();
        playerSpeed = float.Parse(speed);
        string abilityD = PhotonNetwork.room.CustomProperties[CustomizationToServer.ABILITYDURATION].ToString();
        abilityDuration = float.Parse(abilityD);
        string eventO = PhotonNetwork.room.CustomProperties[CustomizationToServer.EVENTOCCURENCE].ToString();
        eventOccurenceRate = float.Parse(eventO);
        string gameL = PhotonNetwork.room.CustomProperties[CustomizationToServer.GAMELENGTH].ToString();
        gameLength = byte.Parse(gameL);
        string pointW = PhotonNetwork.room.CustomProperties[CustomizationToServer.POINTSTOWIN].ToString();
        pointsToWin = short.Parse(pointW);
        string pointK = PhotonNetwork.room.CustomProperties[CustomizationToServer.POINTSTPERKILL].ToString();
        pointsPerKill = short.Parse(pointK);
        string isAmmo = PhotonNetwork.room.CustomProperties[CustomizationToServer.UNLIMITEDAMMO].ToString();
        isAmmoUnlimited = bool.Parse(isAmmo);
        string cEvent = PhotonNetwork.room.CustomProperties[CustomizationToServer.EVENTSLISTED].ToString();
        currentEvents = cEvent;
        string cAddon = PhotonNetwork.room.CustomProperties[CustomizationToServer.ADDONSLISTED].ToString();
        currentAddOns = cAddon;
    }
}