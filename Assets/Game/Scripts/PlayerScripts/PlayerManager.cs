using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    PlayerMovement playerMovement;
    Shooting shooting;
    AnimationManager animationManager;

    Controls controls;
    string saveData;
    bool isSprinting;
    bool isCrouching;
    bool isDisarmed;

    int shotsFired = 5;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        animationManager = GetComponent<AnimationManager>();
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

    public void Disarm()
    {
        shooting.RemoveWeapon();
        isDisarmed = true;
        ///aninmator.disarmed;
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
        
        if (controls.Fire.IsPressed)
            Firing();
            
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
        animationManager.IsCrouching();
    }

    void Firing()
    {
        shooting.Firing();
        //animationManager.IsFiring();

        shotsFired--;

        if (shotsFired < 1)
            Disarm();
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
