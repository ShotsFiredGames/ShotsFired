using UnityEngine;
using System.Collections.Generic;

public class GameCustomization : MonoBehaviour
{
    public List<string> _currentEvents;
    public List<string> _currentAddOns;
    public float _respawnTime;
    public float _playerSpeed;
    public short _playerHealth;
    public bool _isAmmoUnlimited;
    [Range(2, 20)]
    public float _abilityDuration;
    [Range(2, 120)]
    public float _eventOccurenceRate;
    public byte _gameLength;
    public short _pointsToWin;
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
        respawnTime = _respawnTime;
        playerSpeed = _playerSpeed;
        playerHealth = _playerHealth;
        isAmmoUnlimited = _isAmmoUnlimited;
        abilityDuration = _abilityDuration;
        eventOccurenceRate = _eventOccurenceRate;
        gameLength = _gameLength;
        pointsToWin = _pointsToWin;
        pointsPerKill = _pointsPerKill;
    }
}