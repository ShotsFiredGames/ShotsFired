using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class GameCustomization : NetworkBehaviour
{
    public List<string> _currentEvents;
    public List<string> _currentAddOns;
    [SyncVar]
    public float _respawnTime;
    [SyncVar]
    public float _playerSpeed;
    [SyncVar]
    public short _playerHealth;
    [SyncVar]
    public bool _isAmmoUnlimited;
    [SyncVar]
    public float _abilityDuration;
    [SyncVar]
    public float _eventOccurenceRate;
    [SyncVar]
    public byte _gameLength;
    [SyncVar]
    public short _pointsToWin;
    [SyncVar]
    public byte _pointsPerKill;

    public static List<string> currentEvents;
    public static List<string> currentAddOns;
    public static float respawnTime;
    public static float playerSpeed;
    public static short playerHealth;
    public static bool isAmmoUnlimited;
    public static float abilityDuration;
    public static float eventOccurenceRate;
    public static byte gameLength;
    public static short pointsToWin;
    public static byte pointsPerKill;

    private void Awake()
    {
        currentEvents = _currentEvents;
        currentAddOns = _currentAddOns;
        //respawnTime = _respawnTime;
        //playerSpeed = _playerSpeed;
        //playerHealth = _playerHealth;
        //isAmmoUnlimited = _isAmmoUnlimited;
        //abilityDuration = _abilityDuration;
        //eventOccurenceRate = _eventOccurenceRate;
        //gameLength = _gameLength;
        //pointsToWin = _pointsToWin;
        //pointsPerKill = _pointsPerKill;
    }
/*
    public static void SetValues(short pHealth)
    {
        playerHealth = pHealth;
        Debug.LogError("Player Health in Customization: " + playerHealth);
    }
*/
}