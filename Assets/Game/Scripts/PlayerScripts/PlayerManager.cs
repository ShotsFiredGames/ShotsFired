using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
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
            Jumping();

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
        animationManager.IsCrouching();
    }

    void Firing()
    {
        print("Shoot");
        shooting.Firing();
        //animationManager.IsFiring();

        shotsFired--;

        if (shotsFired < 1)
            Disarm();
    }


    public void WeaponPickedUp(GameObject newGun)
    {
        isArmed = true;
        animationManager.Armed();
        shooting.SetWeapon(newGun);
    }

    public void Disarm()
    {
        isArmed = false;
        animationManager.Disarmed();
        //shooting.RemoveWeapon();
    }


    void Dead()
    {
        //animationManager.IsDead();
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
