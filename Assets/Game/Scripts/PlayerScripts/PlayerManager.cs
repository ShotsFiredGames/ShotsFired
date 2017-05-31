using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    PlayerMovement playerMovement;
    AnimationManager animationManager;

    Controls controls;
    string saveData;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        animationManager = GetComponent<AnimationManager>();
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
        //Movement Input
        if (controls.Move.Value != Vector2.zero)
            Moving();
        else
            Idling();

        if (controls.Sprint.WasPressed)
            Sprinting();

        if (controls.Sprint.WasReleased)
            StoppedSprinting();

        if (controls.Jump.WasPressed)
            Jumping();

        if (controls.Crouch.WasPressed)
            Crouching();

        //Shooting Input
        if (controls.Fire.IsPressed)
            Firing();
            
    }

    ////Player States////

    void Moving()
    {
        playerMovement.Move(controls.Move.X, controls.Move.Y);
        animationManager.IsMoving(controls.Move.X, controls.Move.Y);
    }

    void Idling()
    {
        playerMovement.Turn(controls.Look.X);
        animationManager.IsIdle(controls.Look.X);
    }

    void Sprinting()
    {
        playerMovement.Sprinting();
        animationManager.IsSprinting(controls.Move.X, controls.Move.Y);
    }

    void StoppedSprinting()
    {
        playerMovement.StoppedSprinting();
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

    void Crouching()
    {
        playerMovement.Crouch();
        animationManager.IsCrouching(controls.Move.X, controls.Move.Y);
    }

    void Firing()
    {
        //shooting.Fire();
        //animationManager.IsFiring();
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
