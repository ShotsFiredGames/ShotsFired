using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerWrangler : MonoBehaviour
{
    public static PlayerWrangler instance;
    public GameManager gameManager;

    private static Dictionary<string, PlayerManager> players = new Dictionary<string, PlayerManager>();
    private const string PLAYER_ID_PREFIX = "Player ";

    void Awake()
    {
        if (instance != null)
            Debug.LogError("More than one PlayerWrangler in scene.");
        else
            instance = this;
    }

    public static void RegisterPlayer(string netId, PlayerManager player)
    {
        string playerID = PLAYER_ID_PREFIX + netId;
        players.Add(playerID, player);
        player.transform.name = playerID;
        GameManager.instance.AddPlayer(playerID);
    }

    public static void UnRegisterPlayer(string playerID)
    {
        players.Remove(playerID);
    }

    public static PlayerManager GetPlayer(string playerID)
    {
        return players[playerID];
    }

    public static byte GetNumOfPlayers()
    {
        return (byte)players.Count;
    }

    public static PlayerManager[] GetAllPlayers()
    {
        return players.Values.ToArray();
    }
}