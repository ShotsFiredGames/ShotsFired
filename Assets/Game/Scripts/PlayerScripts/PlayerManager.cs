using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    PlayerMovement playerMovement;

    Controls controls;
    string saveData;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
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
        if (controls.Move)
            Moving();
        if (controls.Look)
            Looking();

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
        //animationManager.IsMoving();
    }

    void Looking()
    {
        playerMovement.Turn(controls.Look.X);
        //animationManager.IsLooking();
    }

    void Sprinting()
    {
        playerMovement.Sprinting();
        //animationManager.IsSprinting();
    }

    void StoppedSprinting()
    {
        playerMovement.StoppedSprinting();
        //animationManager.StoppedSprinting();
    }

    void Jumping()
    {
        playerMovement.Jump();
        //animationManager.IsJumping();
    }

    void Crouching()
    {
        playerMovement.Crouch();
        //animationManager.IsCrouching();
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
