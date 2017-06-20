using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class StripTease : AddOn
{
    PlayerManager[] allPlayers;

    public override void StartAddOn()
    {
        allPlayers = PlayerWrangler.GetUnorderedPlayers();

        foreach (PlayerManager player in allPlayers)
        {
            player.CmdDisarm();
        }
    }
}
