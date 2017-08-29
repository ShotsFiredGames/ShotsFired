public class StepItUp : AddOn
{
    public byte newSpeedValue;
    PlayerManager[] allPlayers;

    public override void StartAddOn()
    {
        allPlayers = PlayerWrangler.GetAllPlayers();

        foreach (PlayerManager player in allPlayers)
        {
            if (player != null)
            {
                player.Local_SetSpeed(newSpeedValue);
                player.PhotonView.RPC("RPC_SetSpeed", PhotonTargets.Others, newSpeedValue);
            }
        }
    }

    public override void EndAddOn()
    {
        foreach (PlayerManager player in allPlayers)
        {
            if (player != null)
            {
                player.Local_SetSpeed((byte)GameCustomization.playerSpeed);
                player.PhotonView.RPC("RPC_SetSpeed", PhotonTargets.Others, (byte)GameCustomization.playerSpeed);
            }
        }
    }
}
