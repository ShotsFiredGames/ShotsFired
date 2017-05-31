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
        if(controls.Move)
            playerMovement.Move(controls.Move.X, controls.Move.Y);

        if(controls.Look)
            playerMovement.Turn(controls.Look.X);

        if(controls.Sprint.WasPressed)
            playerMovement.Sprinting();

        if (controls.Sprint.WasReleased)
            playerMovement.StoppedSprinting();

        if(controls.Jump.WasPressed)
            playerMovement.Jump();

        if(controls.Crouch.WasPressed)
            playerMovement.Crouch();
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
