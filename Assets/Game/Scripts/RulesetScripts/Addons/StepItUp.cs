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
        StartCoroutine(SetSpeedValues());
    }

    IEnumerator SetSpeedValues()
    {
        foreach (PlayerManager player in allPlayers)
        {
            if (player != null)
            {
                player.PhotonView.RPC("RPC_SetSpeed", PhotonTargets.All, newSpeedValue);
            }
        }

        yield return new WaitForSeconds(GameCustomization.eventOccurenceRate / 3);

        foreach (PlayerManager player in allPlayers)
        {
            if (player != null)
            {
                player.PhotonView.RPC("RPC_SetSpeed", PhotonTargets.All, GameCustomization.playerSpeed);
            }
        }
    }
}
