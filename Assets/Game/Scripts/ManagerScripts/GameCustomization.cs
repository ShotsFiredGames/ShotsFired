using UnityEngine;

public class GameCustomization : MonoBehaviour
{
    public float _respawnTime;
    public float _playerSpeed;
    public float _sprintSpeed;
    public int _playerHealth;
    public bool _isAmmoUnlimited;

    public static float respawnTime;
    public static float playerSpeed;
    public static float sprintSpeed;
    public static int playerHealth;
    public static bool isAmmoUnlimited;

    private void Awake()
    {
        respawnTime = _respawnTime;
        playerSpeed = _playerSpeed;
        sprintSpeed = _sprintSpeed;
        playerHealth = _playerHealth;
        isAmmoUnlimited = _isAmmoUnlimited;
    }
}