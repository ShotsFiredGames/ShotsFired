using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerManager : NetworkBehaviour
{
    [SyncVar]
    public byte playerID;

    AnimationManager animationManager;
    PlayerMovement playerMovement;
    PlayerCamera playerCamera;
    PlayerHealth playerHealth;
    Shooting shooting;

    Controls controls;
    string saveData;
    bool isSprinting;
    bool isCrouching;
    public bool isArmed;
    public Gun[] guns;

    int shotsFired = 5;

    public bool isDead;

    void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        animationManager = GetComponent<AnimationManager>();
        playerCamera = GetComponent<PlayerCamera>();
        playerHealth.playerID = playerID;
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

    private void Update()
    {
        if (!isLocalPlayer) return;
        if (isDead) return;

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

    private void LateUpdate()
    {
        if (isDead) return;
        playerCamera.Look(controls.Look.Y, controls.Look.X);
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
        StartCoroutine(shooting.Firing());
        //animationManager.IsFiring();
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
        newGun.SetAmmo();

        newGun.SetActiveGun(true);
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
        Debug.LogError("Am Ded from: " + collisionLocation.ToString());
        isDead = true;

        //switch (collisionLocation)
        //{
        //    case CollisionDetection.CollisionFlag.FrontHeadShot:
        //        anim.SetBool("FrontHeadShot", true);
        //        break;
        //    case CollisionDetection.CollisionFlag.BackHeadShot:
        //        anim.SetBool("BackHeadShot", true);
        //        break;
        //    case CollisionDetection.CollisionFlag.Front:
        //        anim.SetBool("Front", true);
        //        break;
        //    case CollisionDetection.CollisionFlag.Back:
        //        anim.SetBool("Back", true);
        //        break;
        //    case CollisionDetection.CollisionFlag.Left:
        //        anim.SetBool("Left", true);
        //        break;
        //    case CollisionDetection.CollisionFlag.Right:
        //        anim.SetBool("Right", true);
        //        break;
        //}
        //animationManager.IsDead();
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
