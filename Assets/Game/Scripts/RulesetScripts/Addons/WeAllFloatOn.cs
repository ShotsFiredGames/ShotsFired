using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeAllFloatOn : AddOn
{
    public double newGravityValue;
    PlayerManager[] allPlayers;

    Coroutine gravityChange;

    void Start()
    {
        //allPlayers = PlayerWrangler.GetAllPlayers();
    }

    public override void StartAddOn()
    {
        allPlayers = PlayerWrangler.GetAllPlayers();
        foreach (PlayerManager player in allPlayers)
        {
            if (player != null)
            {
                player.PhotonView.RPC("RPC_SetGravity", PhotonTargets.All, newGravityValue);
            }
        }
    }

    public override void EndAddOn()
    {
        foreach (PlayerManager player in allPlayers)
        {
            if (player != null)
            {
                player.PhotonView.RPC("RPC_SetGravity", PhotonTargets.All, 9.8);
            }
        }
    }
}
