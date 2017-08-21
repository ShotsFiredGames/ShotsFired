using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jammer : Point
{
    Dictionary<string, bool> jammerInfo;
    RunningAJammer eventBase;
    Color orginalColor;

    public void SetConnection(RunningAJammer jams)
    {
        eventBase = jams;
    }

    public void InitJammer()
    {
        orginalColor = GetComponent<Renderer>().material.color;
        jammerInfo = new Dictionary<string, bool>();
        PlayerManager[] players = PlayerWrangler.GetAllPlayers();

        foreach (PlayerManager play in players)
        {
            jammerInfo.Add(play.name, false);
        }
    }

    public override void PlayerEnters(GameObject player)
    {
        string playerName = player.name;

        if (!jammerInfo[playerName])
        {
            jammerInfo[playerName] = true;

            if (PhotonNetwork.player.NickName.Equals(playerName))
            {
                string faction = player.GetComponent<PlayerManager>().GetFaction();
                gameObject.GetComponent<Renderer>().material.color = PlayerWrangler.GetFactionMaterial(faction).color;
            }

            eventBase.UpdatePlayerJammerInfo(playerName);
        }        
    }

    public void ResetPoint(string playerName)
    {
        jammerInfo[playerName] = false;
        gameObject.GetComponent<Renderer>().material.color = orginalColor;
    }
}
