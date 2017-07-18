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

    public Material blue;
    public Material black;
    public Material red;
    public Material purple;
    public Material green;
    public Material yellow;

    static Material _blue;
    static Material _black;
    static Material _red;
    static Material _purple;
    static Material _green;
    static Material _yellow;

    static int playerCount;

    enum Faction
    {
        Water,
        Earth,
        Fire,
        Air,
        Death,
        Life
    };


    void Awake()
    {
        if (instance != null)
            Debug.LogError("More than one PlayerWrangler in scene.");
        else
            instance = this;

        _blue = blue;
        _black = black;
        _red = red;
        _purple = purple;
        _green = green;
        _yellow = yellow;
    }

    public static void RegisterPlayer(string netId, PlayerManager player)
    {
        playerCount++;
        string playerID = PLAYER_ID_PREFIX + playerCount;
        players.Add(playerID, player);

        switch(playerCount)
        {
            case 1:
                player.SetFaction("Water", _blue);
                break;
            case 2:
                player.SetFaction("Earth", _black);
                break;
            case 3:
                player.SetFaction("Fire", _red);
                break;
            case 4:
                player.SetFaction("Air", _purple);
                break;
            case 5:
                player.SetFaction("Death", _green);
                break;
            case 6:
                player.SetFaction("Life", _yellow);
                break;
        }
        
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