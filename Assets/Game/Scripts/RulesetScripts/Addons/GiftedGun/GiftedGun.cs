using System.Collections.Generic;
using UnityEngine;

public class GiftedGun : AddOn
{
    public List<string> possibleGunNames;
    public PickUpLoacation[] gunSpawns;
    PlayerManager[] allPlayers;

    public override void StartAddOn()
    {
        allPlayers = PlayerWrangler.GetAllPlayers();

        foreach (PlayerManager player in allPlayers)
        {
            if (player != null)
            {
                player.Local_Disarm();
                player.PhotonView.RPC("RPC_Disarm", PhotonTargets.Others);

                if (PhotonNetwork.isMasterClient)
                {
                    int rand = Random.Range(0, possibleGunNames.Count);
                    player.Local_WeaponPickedUp(possibleGunNames[rand]);
                    player.PhotonView.RPC("RPC_WeaponPickedUp", PhotonTargets.Others, possibleGunNames[rand]);
                }
            }
        }

        if (PhotonNetwork.isMasterClient)
        {
            for (byte index = 0; index < gunSpawns.Length; index++)
            {
                ChangeGunSpawns(gunSpawns[index]);
            }
        }
    }

    public override void EndAddOn()
    {
        for (byte index = 0; index < gunSpawns.Length; index++)
        {
            ReturnGunSpawns(gunSpawns[index]);
        }
    }

    void ChangeGunSpawns(PickUpLoacation gunSpawn)
    {
        if (PhotonNetwork.isMasterClient)
        {
            gunSpawn.photonView.RPC("RPC_DestroyItsPickup", PhotonTargets.OthersBuffered);
            Destroy(gunSpawn.activePickUp);
        }

        gunSpawn.SpawnSelectPickup(possibleGunNames[Random.Range(0, possibleGunNames.Count)]);
    }

    void ReturnGunSpawns(PickUpLoacation gunSpawn)
    {
        if (PhotonNetwork.isMasterClient)
        {
            gunSpawn.photonView.RPC("RPC_DestroyItsPickup", PhotonTargets.OthersBuffered);
            Destroy(gunSpawn.activePickUp);
        }

        gunSpawn.SpawnRandomPickup();
    }
}