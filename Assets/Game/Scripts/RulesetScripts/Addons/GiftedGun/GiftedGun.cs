using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftedGun : AddOn
{
    public List<string> possibleGunNames;
    public PickUpLoacation[] gunSpawns;
    public GameObject[] gunThatSpawnFromGunSpawns;
    GameObject[][] previousGunSpawns;
    PlayerManager[] allPlayers;

    public override void StartAddOn()
    {
        previousGunSpawns = new GameObject[gunSpawns.Length][];
        allPlayers = PlayerWrangler.GetAllPlayers();

        foreach (PlayerManager player in allPlayers)
        {
            if(player != null)
            {
                player.PhotonView.RPC("RPC_Disarm", PhotonTargets.All);
                player.PhotonView.RPC("RPC_WeaponPickedUp", PhotonTargets.All, possibleGunNames[Random.Range(0, possibleGunNames.Count)]);
            }
        }

        for (byte index = 0; index < gunSpawns.Length; index++)
        {
            StartCoroutine(ChangeGunSpawns(index, gunSpawns[index]));
        }
    }

    IEnumerator ChangeGunSpawns(byte index, PickUpLoacation gunSpawn)
    {
        if (PhotonNetwork.isMasterClient)
        {
            gunSpawn.photonView.RPC("RPC_DestoryItsPickup", PhotonTargets.OthersBuffered);
            Destroy(gunSpawn.activePickUp);
        }

        previousGunSpawns[index] = gunSpawn.pickUpTypes;
        gunSpawn.pickUpTypes = gunThatSpawnFromGunSpawns;
        gunSpawn.SpawnSelectPickup(0);
        yield return new WaitForSeconds(GameCustomization.eventOccurenceRate / 3);

        if (PhotonNetwork.isMasterClient)
        {
            gunSpawn.photonView.RPC("RPC_DestoryItsPickup", PhotonTargets.OthersBuffered);
            Destroy(gunSpawn.activePickUp);
        }

        gunSpawn.pickUpTypes = previousGunSpawns[index];
        gunSpawn.SpawnRandomPickup();
    }

    [PunRPC]
    void SetNewGunTypes(int index)
    {
        
    }
}
