using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;
using System.Collections.Generic;

public class GameCustomization : NetworkBehaviour
{
    //Version of instance taken from "http://wiki.unity3d.com/index.php/AManagerClass"
    private static GameCustomization s_Instance = null;
    public static GameCustomization instance
    {
        get
        {
            if (s_Instance == null)
            {
                // This is where the magic happens.
                //  FindObjectOfType(...) returns the first AManager object in the scene.
                s_Instance = FindObjectOfType(typeof(GameCustomization)) as GameCustomization;
            }

            // If it is still null, create a new instance
            if (s_Instance == null)
            {
                GameObject obj = new GameObject("GameCustomization");
                s_Instance = obj.AddComponent(typeof(GameCustomization)) as GameCustomization;
                Debug.Log("Could not locate an GameCustomization object. GameCustomization was Generated Automaticly.");
            }

            return s_Instance;
        }
    }

    public SyncListString currentEvents = new SyncListString();
    public SyncListString currentAddOns = new SyncListString();
    [SyncVar]
    public float respawnTime;
    [SyncVar(hook = "OnMySpeed")]
    public float playerSpeed;
    [SyncVar]
    public short playerHealth;
    [SyncVar]
    public bool isAmmoUnlimited;
    [SyncVar]
    public float abilityDuration;
    [SyncVar]
    public float eventOccurenceRate;
    [SyncVar]
    public byte gameLength;
    [SyncVar]
    public short pointsToWin;
    [SyncVar]
    public byte pointsPerKill;

    void OnMySpeed(float speed)
    {
        Debug.LogError("Speed Has changed");
        playerSpeed = speed;
    }
}