using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerManager : NetworkBehaviour
{
    AnimationManager animationManager;
    PlayerMovement playerMovement;
    PlayerCamera playerCamera;
    Juggernaut juggernaut;

    PlayerHealth playerHealth;
    public bool isDead;

    Shooting shooting;

    Controls controls;
    string saveData;

    public bool isArmed;
    public Gun[] guns;
    public LayerMask layermask;
    int shotsFired = 5;

    float yRotationValue;
    GameObject myCamera;

    void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
        playerCamera = GetComponent<PlayerCamera>();
        myCamera = playerCamera.myCamera.gameObject;
    }

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        animationManager = GetComponent<AnimationManager>();

        shooting = GetComponent<Shooting>();
        juggernaut = GetComponentInChildren<Juggernaut>();

        Debug.LogError(juggernaut + " is this");
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

        //RaycastHit hit;
        //if (isArmed && Physics.Raycast(myCamera.transform.position, myCamera.transform.forward, out hit,1000, layermask))
        //{
        //    if(hit.transform.tag.Equals("Collision"))
        //        playerMovement.AimAssist();
        //    else
        //        playerMovement.StopAimAssist();
        //}

        ApplyMovementInput();

        if (controls.Move.Value.Equals(Vector2.zero))
            Idling();
        else
            Moving();

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
                juggernaut.ActivateJuggernaut();
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
