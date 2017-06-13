using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerManager : NetworkBehaviour
{
    AnimationManager animationManager;
    PlayerMovement playerMovement;
    PlayerCamera playerCamera;

    PlayerHealth playerHealth;
    public bool isDead;

    Shooting shooting;

    Controls controls;
    string saveData;
    bool isSprinting;

    public bool isArmed;
    public Gun[] guns;
    int shotsFired = 5;

    float yRotationValue;

    void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
        playerCamera = GetComponent<PlayerCamera>();
    }

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        animationManager = GetComponent<AnimationManager>();

        shooting = GetComponent<Shooting>();
        playerHealth.Init();
    }

    void OnEnable()
    {
        controls = Controls.CreateWithDefaultBindings();
    }

    void OnDisable()
    {
        controls.Destroy();
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer) return;
        if (isDead) return;

        ApplyMovementInput();

        if (controls.Move.Value.Equals(Vector2.zero))
            Idling();
        else
            Moving();

        if (controls.Sprint.IsPressed)
            Sprinting();

        if (controls.Jump.WasPressed)
        {
            Jumping();
        }

        if (controls.Fire)
            Firing();
        else
            StopFiring();

        if (controls.Aim)
        {
            if (!isArmed) return;
            playerCamera.Aim();
            Aim();
        }
        else
        {
            playerCamera.StopAim();
            StopAiming();
        }
    }

    private void LateUpdate()
    {
        if (isDead) return;
        playerCamera.Look(controls.Look.Y);
    }

    public void SetRotationValue(float value)
    {
        yRotationValue = value;
    }
    ////Player States////

    void ApplyMovementInput()
    {
        animationManager.ApplyMovementInput(controls.Move.X, controls.Move.Y, controls.Look.X, yRotationValue);
    }
    void Moving()
    {
        playerMovement.Move(controls.Move.X, controls.Move.Y);
        playerMovement.Turn(controls.Look.X);
        animationManager.IsMoving();
    }

    void Idling()
    {
        playerMovement.Turn(controls.Look.X);
        animationManager.IsIdle();
    }

    void Sprinting()
    {
        playerMovement.Sprinting(controls.Move.X, controls.Move.Y);
        playerMovement.Turn(controls.Look.X);
        animationManager.IsSprinting();
    }

    void Jumping()
    {
        playerMovement.Jump();
        animationManager.IsJumping();
    }

    public void Landed()
    {
        animationManager.IsLanding();
    }

    void Aim()
    {
        if (!isArmed) return;
        animationManager.IsAiming();
    }

    void StopAiming()
    {
        animationManager.StoppedAiming();
    }

    void Firing()
    {
        if (!isArmed) return;
        StartCoroutine(shooting.Firing());
        animationManager.IsFiring();
    }

    void StopFiring()
    {
        animationManager.StoppedFiring();
    }

    [Command]
    public void CmdAbilityPickedUp(string abilityName)
    {
        RpcAbilityPickedUp(abilityName);
    }

    [ClientRpc]
    void RpcAbilityPickedUp(string abilityName)
    {
        switch(abilityName)
        {
            case "SuperBoots":
                playerMovement.SuperBoots();
                break;
            case "Juggernaut":
                //playerMovement.SuperBoots();
                break;
        }
    }

    [Command]
    public void CmdWeaponPickedUp(string gunName)
    {
        RpcWeaponPickedUp(gunName);
    }

    [ClientRpc]
    public void RpcWeaponPickedUp(string gunName)
    {
        isArmed = true;
        animationManager.Armed();
        Gun newGun = FindGun(gunName);

        if (newGun == null) Debug.LogError("Incorrect Name of Gun");

        shooting.SetWeapon(newGun);
        newGun.GetComponent<Gun>().SetAmmo();

        newGun.GetComponent<Gun>().SetActiveGun(true);
    }

    [Command]
    public void CmdDisarm()
    {
        RpcDisarm();
    }

    [ClientRpc]
    public void RpcDisarm()
    {
        isArmed = false;
        animationManager.Disarmed();
        shooting.RemoveWeapon();
        playerCamera.SetFieldOfView(60);
    }

    public void Dead(CollisionDetection.CollisionFlag collisionLocation)
    {
        isDead = true;
        animationManager.IsDead(collisionLocation);
    }

    public void Respawn()
    {
        animationManager.IsRespawning();
        isDead = false;
    }

    Gun FindGun(string gunName)
    {
        foreach (Gun gun in guns)
        {
            if (gun.gunName.Equals(gunName))
                return gun;
        }
        return null;
    }

    void SaveBindings()
    {
        saveData = controls.Save();
        PlayerPrefs.SetString("Bindings", saveData);
    }

    void LoadBindings()
    {
        if (PlayerPrefs.HasKey("Bindings"))
        {
            saveData = PlayerPrefs.GetString("Bindings");
            controls.Load(saveData);
        }
    }
}
