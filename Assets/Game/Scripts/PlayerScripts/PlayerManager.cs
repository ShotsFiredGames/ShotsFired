using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerManager : NetworkBehaviour
{
    AnimationManager animationManager;
    PlayerMovement playerMovement;
    PlayerCamera playerCamera;
    Shooting shooting;

    Controls controls;
    string saveData;
    bool isSprinting;
    bool isCrouching;
    public bool isArmed;
    public Gun[] guns;

    int shotsFired = 5;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        animationManager = GetComponent<AnimationManager>();
        playerCamera = GetComponent<PlayerCamera>();
        shooting = GetComponent<Shooting>();
    }

    void OnEnable()
    {
        controls = Controls.CreateWithDefaultBindings();
    }

    void OnDisable()
    {
        controls.Destroy();
    }

    private void Update()
    {
        if (!isLocalPlayer) return;

        ApplyMovementInput();

        if (controls.Crouch.WasPressed)
            isCrouching = !isCrouching;

        if (!isCrouching && controls.Move.Value.Equals(Vector2.zero))
            Idling();
        else if (isCrouching && controls.Move.Value.Equals(Vector2.zero))
            CrouchIdling();

        if (isCrouching && !controls.Move.Value.Equals(Vector2.zero))
            Crouching();
        else if (!isCrouching && !controls.Move.Value.Equals(Vector2.zero))
            Moving();

        if (controls.Sprint.IsPressed)
            Sprinting();

        if (controls.Jump.WasPressed)
        {
            Jumping();
            CmdWeaponPickedUp("Scorpion");
        }        

        if (controls.Fire)
            Firing();

        if(controls.Aim)
        {
            playerCamera.Aim();
        }
        else
        {
            playerCamera.StopAim();
        }

    }

    ////Player States////

    void ApplyMovementInput()
    {
        animationManager.ApplyMovementInput(controls.Move.X, controls.Move.Y, controls.Look.X);
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

    void CrouchIdling()
    {
        playerMovement.Turn(controls.Look.X);
        animationManager.IsCrouchIdle();
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

    void Crouching()
    {
        playerMovement.Move(controls.Move.X, controls.Move.Y);
        playerMovement.Turn(controls.Look.X);
        animationManager.IsCrouching();
    }

    void Firing()
    {
        print("Shoot");
        shooting.Firing();
        //animationManager.IsFiring();
    }

    [Command]
    public void CmdWeaponPickedUp(string gunName)
    {
        Debug.LogError("We've pickup up a " + gunName);
        RpcWeaponPickedUp(gunName);
    }

    [ClientRpc]
    public void RpcWeaponPickedUp(string gunName)
    {
        isArmed = true;
        animationManager.Armed();
        GameObject newGun = FindGun(gunName);

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


    void Dead()
    {
        //animationManager.IsDead();
    }

    GameObject FindGun(string gunName)
    {
        Debug.LogError("There are" + guns.Length + " guns " + gunName);
        foreach (Gun gun in guns)
        {
            if (gun.gunName.Equals(gunName))
                return gun.gameObject;
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
