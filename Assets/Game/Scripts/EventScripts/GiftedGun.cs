//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftedGun : AddOn
{
    public List<string> possibleGunNames;
    PlayerManager[] allPlayers;

    public override void StartAddOn()
    {
        allPlayers = PlayerWrangler.GetUnorderedPlayers();

        foreach (PlayerManager player in allPlayers)
        {
            player.CmdDisarm();
            player.CmdWeaponPickedUp(possibleGunNames[Random.Range(0, possibleGunNames.Count)]);
        }
    }
}
