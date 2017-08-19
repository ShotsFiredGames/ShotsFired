using System;

public class HugeLoad : AddOn
{
    PlayerManager[] allPlayers;

    public override void StartAddOn()
    {
        allPlayers = PlayerWrangler.GetAllPlayers();

        foreach (PlayerManager player in allPlayers)
        {
            player.GetComponent<PhotonView>().RPC("RPC_CancelAbility", PhotonTargets.All);
            foreach (PickUp.AbilityType ability in Enum.GetValues(typeof(PickUp.AbilityType)))
            {
                if (player != null)
                    player.PhotonView.RPC("RPC_AbilityPickedUp", PhotonTargets.All, ability.ToString());
            }
        }
    }

    public override void EndAddOn()
    {
        ///
    }

}