using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeAllFloatOn : AddOn
{
    public double newGravityValue;
    PlayerManager[] allPlayers;

    Coroutine gravityChange;

    public override void StartAddOn()
    {
        allPlayers = PlayerWrangler.GetAllPlayers();

        if (gravityChange != null)
            StopCoroutine(gravityChange);

        gravityChange = StartCoroutine(SetSpeedValues());
    }

    IEnumerator SetSpeedValues()
    {
        foreach (PlayerManager player in allPlayers)
        {
            if (player != null)
            {
                player.PhotonView.RPC("RPC_SetGravity", PhotonTargets.All, newGravityValue);
            }
        }

        yield return new WaitForSeconds(GameCustomization.eventOccurenceRate / 3);

        foreach (PlayerManager player in allPlayers)
        {
            if (player != null)
            {
                player.PhotonView.RPC("RPC_SetGravity", PhotonTargets.All, 9.8);
            }
        }
    }
}
