using UnityEngine;
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
    public CameraShake camShake;
    public AudioSource jumpSource;
    public AudioClip jumpSound;
    public AudioClip landSound;

    float yRotationValue;
    GameObject myCamera;
    Gun oldGun;

    public static bool isWalking;
    public static bool isSprinting;
    [HideInInspector]
    public bool canMove;
    [HideInInspector]
    public bool hasFlag;
    public Image haveFlag;
    public AudioMixer gameMixer;
    [HideInInspector]
    public int sprintFoV;

    GunBob gunBob;
    HeadBob headBob;
    bool jumped;

    void Awake()
    {
        PhotonNetwork.OnEventCall += EnableMovement;
        playerHealth = GetComponent<PlayerHealth>();
        playerCamera = GetComponent<PlayerCamera>();
        myCamera = playerCamera.myCamera.gameObject;
        gunBob = GetComponentInChildren<GunBob>();
        headBob = GetComponentInChildren<HeadBob>();
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

    void EnableMovement(byte eventcode, object content, int senderid)
    {
        if (eventcode == 0)
        {
            canMove = (bool)content;
            shooting.UnArmed();
        }
    }

    [PunRPC]
    public void RPC_SetFactionPlayer(string myFaction)
    {
        faction = myFaction;
        factionColor = PlayerWrangler.GetFactionMaterial(faction);
        rend.material = factionColor;
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

        if (!controls.Move && !controls.Sprint)
            Idling();
        else if (controls.Move && !controls.Sprint)
            Moving();
        else if (controls.Move && controls.Sprint && !controls.Aim)
            Sprinting();
        else if (controls.Move && controls.Sprint && controls.Aim)
            Moving();
        else
            Idling();

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

    public void ShakeCam(float shakeIntensity, float shakeDecay)
    {
        camShake.DoShake(shakeIntensity, shakeDecay);
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
        isSprinting = false;
        isWalking = true;
        playerMovement.Turn(controls.Look.X);
        if (!canMove) return;
        playerMovement.Move(controls.Move.X, controls.Move.Y);
        animationManager.IsMoving();
        StoppedSprinting();
    }

    void Idling()
    {
        isWalking = false;
        isSprinting = false;
        playerMovement.Turn(controls.Look.X);
        animationManager.IsIdle();
        StoppedSprinting();
    }

    void Sprinting()
    {
        isWalking = false;
        isSprinting = true;
        playerMovement.Turn(controls.Look.X);
        if (!canMove) return;
        playerMovement.Move(controls.Move.X, controls.Move.Y);
        playerMovement.Sprint();
        animationManager.IsSprinting();
        sprintFoV = 10;
    }

    void StoppedSprinting()
    {
        isSprinting = false;

        playerMovement.StopSprint();
        animationManager.StoppedSprinting();
        sprintFoV = 0;
    }

    void Jumping()
    {
        if (!canMove) return;
        isWalking = false;
        isSprinting = false;
        jumped = true;
        playerMovement.Jump();
        animationManager.IsJumping();
        jumpSource.clip = jumpSound;
        jumpSource.Play();
    }
    public void Landed()
    {
        if (playerMovement.canShake)
            ShakeCam(.225f, .12f);

        isWalking = false;
        isSprinting = false;

        jumped = false;
        animationManager.IsLanding();
        jumpSource.clip = landSound;
        jumpSource.Play();
    }

    public void Falling()
    {
        isWalking = false;
        isSprinting = false;
        animationManager.IsFalling();
        jumped = false;
    }

    void Aim()
    {
        if (!isArmed) return;
        if (!isAiming)
        {
            isAiming = true;
            gunBob.Aiming(true);
            headBob.Aiming(true);
            shooting.Aiming();
            animationManager.IsAiming();
        }

        playerCamera.Aim();
    }

    void StopAiming()
    {
        if (isAiming)
        {
            gunBob.Aiming(false);
            headBob.Aiming(false);
            shooting.NotAiming();
            animationManager.StoppedAiming();
            isAiming = false;
        }

        playerCamera.StopAim();
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
        
       // animationManager.SetGunAnimator(newGun.anim);
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
            FlagManager.instance.photonView.RPC("RPC_FlagDropped", PhotonTargets.All, name);

        playerMovement.CancelSpeedBoost();

        isDead = true;
        photonView.RPC("RPC_Disarm", PhotonTargets.All);
        photonView.RPC("RPC_CancelAbility", PhotonTargets.All);
        animationManager.IsDead(collisionLocation);
        PhotonView gmPhotonView = GameManager.instance.PhotonView;
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
