using UnityEngine;
using System.Collections.Generic;

public class GameCustomization : MonoBehaviour
{
    public List<string> _currentEvents;
    public List<string> _currentAddOns;
    public float _respawnTime;
    public float _playerSpeed;
    public int _playerHealth;
    public bool _isAmmoUnlimited;
    [Range(2, 20)]
    public float _abilityDuration;
    [Range(30, 120)]
    public float _eventOccurenceRate;
    public int _gameLength;
    public int _pointsToWin;
    public int _pointsPerKill;


    public static List<string> currentEvents;
    public static List<string> currentAddOns;
    public static float respawnTime;
    public static float playerSpeed;
    public static int playerHealth;
    public static bool isAmmoUnlimited;
    public static float abilityDuration;
    public static float eventOccurenceRate;
    public static int gameLength;
    public static int pointsToWin;
    public static int pointsPerKill;

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