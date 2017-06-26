
public class StripTease : AddOn
{
    PlayerManager[] allPlayers;

    public override void StartAddOn()
    {
        allPlayers = PlayerWrangler.GetAllPlayers();

        foreach (PlayerManager player in allPlayers)
        {
            if(player != null)
            player.CmdDisarm();
        }
    }
}