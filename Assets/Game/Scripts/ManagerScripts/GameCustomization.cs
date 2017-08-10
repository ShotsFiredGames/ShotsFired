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
    [Range(2, 1000)]
    public float _eventOccurenceRate;
    public byte _gameLength;
    public short _pointsToWin;
    public short _pointsPerKill;

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
    public static short pointsPerKill;

    private void Awake()
    {        
        currentEvents = _currentEvents;
        currentAddOns = _currentAddOns;
        //respawnTime = _respawnTime;
        playerSpeed = _playerSpeed;
        //playerHealth = _playerHealth;
        isAmmoUnlimited = _isAmmoUnlimited;
        abilityDuration = _abilityDuration;
        eventOccurenceRate = _eventOccurenceRate;
        gameLength = _gameLength;
        pointsToWin = _pointsToWin;
        pointsPerKill = _pointsPerKill;

        //print(PhotonNetwork.room.CustomProperties.TryGetValue(CustomizationToServer.PLAYERHEALTH, out playerHealth) + " whatever is in customprops");
        string hp = PhotonNetwork.room.CustomProperties[CustomizationToServer.PLAYERHEALTH].ToString();
        playerHealth = short.Parse(hp);
        print("Player health: " + playerHealth);
        string respawn = PhotonNetwork.room.CustomProperties[CustomizationToServer.RESPAWNTIME].ToString();
        respawnTime = float.Parse(respawn);
    }
}