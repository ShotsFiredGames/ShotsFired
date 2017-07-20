using System.Collections.Generic;
using UnityEngine;

public class GiftedGun : AddOn
{
    public List<string> possibleGunNames;
    PlayerManager[] allPlayers;

    public override void StartAddOn()
    {
        allPlayers = PlayerWrangler.GetAllPlayers();

        foreach (PlayerManager player in allPlayers)
        {
            if(player != null)
            {
                player.CmdDisarm();
                player.CmdWeaponPickedUp(possibleGunNames[Random.Range(0, possibleGunNames.Count)]);
            }
        }
    }
}
