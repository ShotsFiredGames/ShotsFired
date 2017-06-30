using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Networking;

public class PlayerManager : NetworkBehaviour
{
    AnimationManager animationManager;
    PlayerMovement playerMovement;
    PlayerCamera playerCamera;
    Juggernaut juggernaut;
    GameManager gameManager;

    PlayerHealth playerHealth;
    public bool isDead;

    Shooting shooting;

    Controls controls;
    string saveData;

    public bool isArmed;
    bool isAiming;
    bool isFiring;
    public Gun[] guns;
    public LayerMask layermask;
    byte shotsFired = 5;

    float yRotationValue;
    GameObject myCamera;
    Gun oldGun;

    public bool lockCursor;
    private bool m_cursorIsLocked = true;

    [HideInInspector]
    public bool hasFlag;    
    CaptureTheFlag captureTheFlag;
    bool currentlyBeingReaped;
    public AudioMixer gameMixer;

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
        captureTheFlag = GameObject.Find("CaptureTheFlag").GetComponent<CaptureTheFlag>();

        shooting = GetComponent<Shooting>();
        juggernaut = GetComponentInChildren<Juggernaut>();

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

    void FixedUpdate()
    {
        RaycastHit hit;
        if (isArmed && Physics.Raycast(myCamera.transform.position, myCamera.transform.forward, out hit, Mathf.Infinity, layermask))
            playerMovement.AimAssist();
        else
            playerMovement.StopAimAssist();
    }

    private void Update()
    {
        UpdateCursorLock();
        if (!isLocalPlayer) return;
        if (isDead) return;        

        ApplyMovementInput();

        if (controls.Move.Value.Equals(Vector2.zero))
            Idling();
        else
            Moving();

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
        
        if(gameManager != null)
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
            if(GameObject.Find("GameManager") != null)
                gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
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
        if(!isAiming)
        {
            isAiming = true;
            playerCamera.Aim();
            animationManager.IsAiming();
        }        
    }

    void StopAiming()
    {
        if(isAiming)
        {
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
        animationManager.IsFiring();
    }

    [ClientCallback]
    void StopFiring()
    {
        if (!isFiring) return;
        isFiring = false;
        shooting.CmdStopFiring();
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
            case "Juggernaut":
                juggernaut.ActivateJuggernaut();
                playerMovement.SuperBoots();
                break;
            case "Overcharged":
                shooting.ActivateOvercharged();
                break;
        }
    }

    [Command]
    public void CmdCancelAbility()
    {
        RpcCancelAbility();
    }

    [ClientRpc]
    void RpcCancelAbility()
    {
        playerMovement.CancelSuperBoots();
        juggernaut.CancelJuggernaut();
        shooting.CancelOvercharged();
    }

    [Command]
    public void CmdWeaponPickedUp(string gunName)
    {
        RpcWeaponPickedUp(gunName);
    }

    [ClientRpc]
    public void RpcWeaponPickedUp(string gunName)
    {
        if(oldGun != null)
        oldGun.SetActiveGun(false);

        isArmed = true;
        animationManager.Armed();
        Gun newGun = FindGun(gunName);
        oldGun = newGun;

        if (newGun == null) Debug.LogError("Incorrect Name of Gun");

        shooting.SetWeapon(newGun);
        newGun.SetAmmo();

        newGun.SetActiveGun(true);

        if (isAiming)
            Aim();
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
        shooting.RemoveWeapon();
        animationManager.Disarmed();
        playerCamera.SetFieldOfView(60);
    }

    
    public void Dead(string damageSource, CollisionDetection.CollisionFlag collisionLocation)
    {
        if (hasFlag)
            captureTheFlag.CmdFlagDropped();

        playerMovement.CancelSpeedBoost();

        isDead = true;
        CmdDisarm();
        CmdCancelAbility();
        animationManager.IsDead(collisionLocation);
        GameManager.instance.CmdAddScore(damageSource, GameCustomization.pointsPerKill);
        AnnouncerManager.instance.PlayDeathClip(damageSource);
    }

    public void Respawn()
    {
        if(currentlyBeingReaped)
        {
            gameMixer.SetFloat("ReaperVolume", -80);
            currentlyBeingReaped = false;
        }

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

    public void ActivateReaperEffects()
    {
        currentlyBeingReaped = true;
        gameMixer.SetFloat("ReaperVolume", 20);
        //Handle any camera effects
    }

    public void UpdateCursorLock()
    {
        //if the user set "lockCursor" we check & properly lock the cursos
        if (lockCursor)
            InternalLockUpdate();
    }

    private void InternalLockUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            m_cursorIsLocked = false;
        else if (Input.GetMouseButtonUp(0))
            m_cursorIsLocked = true;

        if (m_cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (!m_cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
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
