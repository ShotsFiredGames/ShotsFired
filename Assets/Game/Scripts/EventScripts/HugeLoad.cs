using System;

public class HugeLoad : AddOn
{
    PlayerManager[] allPlayers;

    public override void StartAddOn()
    {
        allPlayers = PlayerWrangler.GetAllPlayers();

        foreach (PlayerManager player in allPlayers)
        {
            player.CmdCancelAbility();
            foreach (PickUp.AbilityType ability in Enum.GetValues(typeof(PickUp.AbilityType)))
            {
                if(player != null)
                player.CmdAbilityPickedUp(ability.ToString());
            }
        }
    }

}
