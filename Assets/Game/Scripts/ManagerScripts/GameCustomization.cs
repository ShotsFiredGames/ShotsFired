using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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

    public static float s_respawnTime;
    public static float s_playerSpeed;
    public static short s_playerHealth;
    public static bool s_isAmmoUnlimited;
    public static float s_abilityDuration;
    public static float s_eventOccurenceRate;
    public static byte s_gameLength;
    public static short s_pointsToWin;
    public static byte s_pointsPerKill;

    void OnMySpeed(float speed)
    {
        Debug.LogError("Speed was : " + playerSpeed);
        playerSpeed = speed;
        Debug.LogError("Speed Has changed: " + playerSpeed);
    }

    void Start()
    {
        s_Instance = this;
        SceneHasChanged();
        //Debug.LogError("Aekae");
    }

    public void SceneHasChanged()
    {
        respawnTime = s_respawnTime;
        playerHealth = s_playerHealth;
        isAmmoUnlimited = s_isAmmoUnlimited;
        abilityDuration = s_abilityDuration;
        eventOccurenceRate = s_eventOccurenceRate;
        gameLength = s_gameLength;
        pointsPerKill = s_pointsPerKill;
        pointsToWin = s_pointsToWin;

        Debug.LogError("Player Speed is: " + playerSpeed);
    }

    [Command]
    public float CmdChangeSpeed()
    {
        float newSpeed = RpcChangeSpeed();
        return newSpeed;
    }

    [ClientRpc]
    float RpcChangeSpeed()
    {
        return s_playerSpeed;
    }
}