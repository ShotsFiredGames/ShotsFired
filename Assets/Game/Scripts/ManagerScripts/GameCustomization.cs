using UnityEngine;

public class GameCustomization : MonoBehaviour
{
    public float _playerSpeed;
    public float _sprintSpeed;
    public bool _isAmmoUnlimited;

    public static float playerSpeed;
    public static float sprintSpeed;
    public static bool isAmmoUnlimited;

    private void Start()
    {
        playerSpeed = _playerSpeed;
        sprintSpeed = _sprintSpeed;
        isAmmoUnlimited = _isAmmoUnlimited;
    }
}
