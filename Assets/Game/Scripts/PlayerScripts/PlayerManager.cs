﻿using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PlayerManager : Photon.MonoBehaviour, IPunObservable
{
    public PhotonView PhotonView { get; private set; }
    AnimationManager animationManager;
    PlayerMovement playerMovement;
    PlayerCamera playerCamera;
    Juggernaut juggernaut;
    GameManager gameManager;

    PlayerHealth playerHealth;
    public bool isDead;
    string faction;
    Material factionColor;

    Shooting shooting;

    Controls controls;
    string saveData;

    public Renderer rend;
    public bool isArmed;
    bool isAiming;
    bool isFiring;
    public Gun[] guns;
    public LayerMask layermask;

    float yRotationValue;
    GameObject myCamera;
    Gun oldGun;

    [HideInInspector]
    public bool hasFlag;
    public Image haveFlag;
    public AudioMixer gameMixer;

    void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
        playerCamera = GetComponent<PlayerCamera>();
        myCamera = playerCamera.myCamera.gameObject;
        PhotonView = photonView;
    }

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        animationManager = GetComponent<AnimationManager>();

        shooting = GetComponent<Shooting>();
        juggernaut = GetComponentInChildren<Juggernaut>();

        playerHealth.Init();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(isFiring);
        }
        else
        {
            this.isFiring = (bool)stream.ReceiveNext();
        }
    }

    public void SetFaction(string myFaction, Material myColor)
    {
        faction = myFaction;
        factionColor = myColor;
        rend.material = myColor;
    }

    public string GetFaction()
    {
        return faction;
    }

    public Material GetFactionColor()
    {
        return factionColor;
    }

    void OnEnable()
    {
        controls = Controls.CreateWithDefaultBindings();
    }

    void OnDisable()
    {
        controls.Destroy();
    }

    void FixedUpdate()
    {
        if (!photonView.isMine) return;
        RaycastHit hit;
        if (isArmed && Physics.BoxCast(myCamera.transform.position, (Vector3.one * .5f), myCamera.transform.forward * 1000, out hit, Quaternion.identity, Mathf.Infinity, layermask))
            playerMovement.AimAssist();
        else
            playerMovement.StopAimAssist();
    }

    private void Update()
    {
        if (!photonView.isMine) return;
        
        haveFlag.gameObject.SetActive(hasFlag);

        if (gameManager != null)
        {
            if (PlatformManager.systemType.Equals(PlatformManager.SystemType.PC))
            {
                if (controls.ScoreBoard.IsPressed && !gameManager.isActive)
                    gameManager.ScoreBoard(true);
                else if (controls.ScoreBoard.WasReleased && gameManager.isActive)
                    gameManager.ScoreBoard(false);
            }
            else
            {
                if (controls.ScoreBoard.WasPressed && !gameManager.isActive)
                    gameManager.ScoreBoard(true);
                else if (controls.ScoreBoard.WasPressed && gameManager.isActive)
                    gameManager.ScoreBoard(false);
            }
        }
        else
        {
            if (GameObject.Find("GameManager") != null)
                gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }

        if (isDead) return;

        ApplyMovementInput();

        if (controls.Move.Value.Equals(Vector2.zero))
            Idling();
        else
            Moving();

        if (controls.Sprint && !isAiming)
            Sprinting();
        else
            StoppedSprinting();

        if (controls.Jump.WasPressed)
            Jumping();

        if (controls.Fire)
            Firing();
        else
            StopFiring();

        if (isArmed && controls.Aim && shooting.currentGun.canAim)
            Aim();
        else
            StopAiming();

      
    }

    private void LateUpdate()
    {
        if (!photonView.isMine) return;
        if (isDead) return;
        playerCamera.Look(controls.Look.Y);
    }

    public void SetRotationValue(float value)
    {
        yRotationValue = value;
    }


    public void EnteredSpeedBoost()
    {
        playerMovement.SpeedBoosted();
    }

    public void SpeedBoost()
    {
        playerMovement.ActivateSpeedBoost();
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
        playerMovement.Sprint();
        animationManager.IsSprinting();
    }

    void StoppedSprinting()
    {
        playerMovement.StopSprint();
        animationManager.StoppedSprinting();
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

    public void Falling()
    {
        animationManager.IsFalling();
    }

    void Aim()
    {
        if (!isArmed) return;
        if (!isAiming)
        {
            isAiming = true;
            shooting.Aiming();
            playerCamera.Aim();
            animationManager.IsAiming();
        }
    }

    void StopAiming()
    {
        if (isAiming)
        {
            shooting.NotAiming();
            playerCamera.StopAim();
            animationManager.StoppedAiming();
            isAiming = false;
        }
    }

    void Firing()
    {
        if (!isArmed) return;
        if (!isFiring)
            isFiring = true;
        StartCoroutine(shooting.Firing());
    }

    public void FireAnimation()
    {
        animationManager.IsFiring();
    }

    void StopFiring()
    {
        if (!isFiring) return;
        isFiring = false;
        shooting.StopFiring();
        animationManager.StoppedFiring();
    }

    [PunRPC]
    public void RPC_AbilityPickedUp(string abilityName)
    {
        switch (abilityName)
        {
            case "Juggernaut":
                juggernaut.ActivateJuggernaut();
                playerMovement.SuperBoots();
                break;
            case "Overcharged":
                shooting.ActivateOvercharged();
                break;
        }
    }

    [PunRPC]
    public void RPC_CancelAbility()
    {
        playerMovement.CancelSuperBoots();
        juggernaut.CancelJuggernaut();
        shooting.CancelOvercharged();
    }

    [PunRPC]
    public void RPC_WeaponPickedUp(string gunName)
    {
        if(oldGun != null)
        oldGun.SetActiveGun(false);

        shooting.UnArmed();
        if (oldGun != null)
        {
            if(oldGun.anim != null && oldGun.anim.gameObject.activeSelf)
                oldGun.anim.SetBool("IsFiring", false);
            oldGun.SetActiveGun(false);
        }

        isArmed = true;
        animationManager.Armed();
        Gun newGun = FindGun(gunName);
        oldGun = newGun;

        if (newGun == null) Debug.LogError("Incorrect Name of Gun");
        
        animationManager.SetGunAnimator(newGun.anim);
        shooting.SetWeapon(newGun);

        shooting.NotAiming();
        newGun.SetAmmo();

        newGun.SetActiveGun(true);
        isAiming = false;
        playerCamera.StopAim();
        animationManager.StoppedAiming();
    }

    [PunRPC]
    public void RPC_Disarm()
    {
        isArmed = false;
        shooting.UnArmed();
        shooting.RemoveWeapon();
        animationManager.Disarmed();
        playerCamera.SetFieldOfView(60);
    }
    
    public void Dead(string damageSource, CollisionDetection.CollisionFlag collisionLocation)
    {
        if (hasFlag)
            photonView.RPC("RPC_FlagDropped", PhotonTargets.All, name);

        playerMovement.CancelSpeedBoost();

        isDead = true;
        photonView.RPC("RPC_Disarm", PhotonTargets.All);
        photonView.RPC("RPC_CancelAbility", PhotonTargets.All);
        animationManager.IsDead(collisionLocation);
        PhotonView gmPhotonView = gameManager.PhotonView;
        if (gmPhotonView != null)
            gmPhotonView.RPC("RPC_AddScore", PhotonTargets.All, damageSource, GameCustomization.pointsPerKill);
    }

    public void Respawn()
    {
        ReaperEffectsActivate(false);
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

    public void ReaperEffectsActivate(bool isActive)
    {
        if (isActive)
        {
            gameMixer.SetFloat("ReaperVolume", 20);
            //Handle any camera effects
        }
        else
        {
            gameMixer.SetFloat("ReaperVolume", -80);
            //turn off effects
        }

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
