using System;

public class HugeLoad : AddOn
{
    PlayerManager[] allPlayers;

    public override void StartAddOn()
    {
        allPlayers = PlayerWrangler.GetAllPlayers();

        foreach (PlayerManager player in allPlayers)
        {
            player.Local_CancelAbility();
            player.GetComponent<PhotonView>().RPC("RPC_CancelAbility", PhotonTargets.Others);
            foreach (PickUp.AbilityType ability in Enum.GetValues(typeof(PickUp.AbilityType)))
            {
                if (player != null)
                {
                    player.Local_AbilityPickedUp(ability.ToString());
                    player.PhotonView.RPC("RPC_AbilityPickedUp", PhotonTargets.Others, ability.ToString());
                }
            }
        }
    }

    public override void EndAddOn()
    {
        ///
    }
}