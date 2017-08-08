using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerWrangler : Photon.MonoBehaviour
{
    public static PlayerWrangler instance;
    public GameManager gameManager;

    private static Dictionary<string, PlayerManager> players = new Dictionary<string, PlayerManager>();
    private const string PLAYER_ID_PREFIX = "Player ";

    public Material blue;
    public Material black;
    public Material red;
    public Material white;
    public Material green;
    public Material yellow;

    static Material _blue;
    static Material _black;
    static Material _red;
    static Material _white;
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
        _white = white;
        _green = green;
        _yellow = yellow;
    }

    public static void RegisterPlayer(string _playerId, PlayerManager player)
    {
        playerCount++;
        player.transform.name = _playerId;
        players.Add(_playerId, player);

        switch(playerCount)
        {
            case 1:
                player.GetComponent<PlayerManager>().SetFaction("Water", _blue);
                break;
            case 2:
                player.GetComponent<PlayerManager>().SetFaction("Earth", _black);
                break;
            case 3:
                player.GetComponent<PlayerManager>().SetFaction("Fire", _red);
                break;
            case 4:
                player.GetComponent<PlayerManager>().SetFaction("Air", _white);
                break;
            case 5:
                player.GetComponent<PlayerManager>().SetFaction("Death", _green);
                break;
            case 6:
                player.GetComponent<PlayerManager>().SetFaction("Life", _yellow);
                break;
        }
        
        GameManager.instance.AddPlayer(_playerId);
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

    public static Material GetFactionMaterial(string factionName)
    {
        switch(factionName)
        {
            case "Water":
                return _blue;
            case "Earth":
                return _black;
            case "Fire":
                return _red;
            case "Air":
                return _white;
            case "Death":
                return _green;
            case "Life":
                return _yellow;
            default:
                return null;
        }
    }
}