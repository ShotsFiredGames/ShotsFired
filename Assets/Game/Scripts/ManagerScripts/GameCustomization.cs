using UnityEngine;
using System.Collections.Generic;

public class GameCustomization : MonoBehaviour
{
    public List<GameEvent> _currentEvents;
    public float _respawnTime;
    public float _playerSpeed;
    public int _playerHealth;
    public bool _isAmmoUnlimited;
    public float _abilityDuration;
    public float _eventOccurenceRate;
    public int _gameLength;
    public int _pointsToWin;
    public int _pointsPerKill;


    public static List<GameEvent> currentEvents;
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