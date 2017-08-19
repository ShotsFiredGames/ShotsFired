using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepItUp : AddOn
{
    public byte newSpeedValue;
    PlayerManager[] allPlayers;

    public override void StartAddOn()
    {
        allPlayers = PlayerWrangler.GetAllPlayers();

        foreach (PlayerManager player in allPlayers)
        {
            if (player != null)
            {
                player.PhotonView.RPC("RPC_SetSpeed", PhotonTargets.All, newSpeedValue);
            }
        }
    }

    public override void EndAddOn()
    {
        foreach (PlayerManager player in allPlayers)
        {
            if (player != null)
            {
                player.PhotonView.RPC("RPC_SetSpeed", PhotonTargets.All, (byte)GameCustomization.playerSpeed);
            }
        }
    }
}
