using UnityEngine;

public class GameCustomization : MonoBehaviour
{
    public float _respawnTime;
    public float _playerSpeed;
    public int _playerHealth;
    public bool _isAmmoUnlimited;
    public float _abilityDuration;
    public int _gameLength;
    public int _pointsToWin;
    public int _pointsPerKill;

    public static float respawnTime;
    public static float playerSpeed;
    public static int playerHealth;
    public static bool isAmmoUnlimited;
    public static float abilityDuration;
    public static int gameLength;
    public static int pointsToWin;
    public static int pointsPerKill;

    private void Awake()
    {
        respawnTime = _respawnTime;
        playerSpeed = _playerSpeed;
        playerHealth = _playerHealth;
        isAmmoUnlimited = _isAmmoUnlimited;
        abilityDuration = _abilityDuration;
        gameLength = _gameLength;
        pointsToWin = _pointsToWin;
        pointsPerKill = _pointsPerKill;
    }
}