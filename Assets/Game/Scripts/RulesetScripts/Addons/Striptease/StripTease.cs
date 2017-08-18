using System;

public class StripTease : AddOn
{
    PlayerManager[] allPlayers;

    void Start()
    {
       
    }

    public override void StartAddOn()
    {
        allPlayers = PlayerWrangler.GetAllPlayers();
        foreach (PlayerManager player in allPlayers)
        {
            if(player != null)
                player.PhotonView.RPC("RPC_Disarm", PhotonTargets.All);
        }
    }

    public override void EndAddOn()
    {
        //
    }
}