using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HugeLoad : AddOn
{
    PlayerManager[] allPlayers;

    public override void StartAddOn()
    {
        allPlayers = PlayerWrangler.GetUnorderedPlayers();

        foreach (PlayerManager player in allPlayers)
        {
            player.CmdCancelAbility();
            foreach (PickUp.AbilityType ability in Enum.GetValues(typeof(PickUp.AbilityType)))
            {
                player.CmdAbilityPickedUp(ability.ToString());
            }
        }
    }

}
