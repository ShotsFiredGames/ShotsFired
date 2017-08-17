using System.Collections;
using UnityEngine;

public class GiftedGun : AddOn
{
    public List<string> possibleGunNames;
    public GameObject[] gunSpawns;
    public GameObject[] gunThatSpawnFromGunSpawns;
    GameObject[] previousGunSpawns;
    PlayerManager[] allPlayers;

    public override void StartAddOn()
    {
        allPlayers = PlayerWrangler.GetAllPlayers();

        foreach (PlayerManager player in allPlayers)
        {
            if(player != null)
            {
                player.PhotonView.RPC("RPC_Disarm", PhotonTargets.All);
                player.PhotonView.RPC("RPC_WeaponPickedUp", PhotonTargets.All, possibleGunNames[Random.Range(0, possibleGunNames.Count)]);
            }
        }
    }
/*
    IEnumerator ChangeGunSpawns()
    {

    }
    */
}
