using UnityEngine;

public class WeAllFloatOn : AddOn
{
    public float newGravityValue;
    PlayerManager[] allPlayers;

    Coroutine gravityChange;

    public override void StartAddOn()
    {
        allPlayers = PlayerWrangler.GetAllPlayers();
        foreach (PlayerManager player in allPlayers)
        {
            if (player != null)
            {
                player.Local_SetGravity(newGravityValue);
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
                player.Local_SetGravity(9.8f);
                player.PhotonView.RPC("RPC_SetGravity", PhotonTargets.All, 9.8f);
            }
        }
    }
}
