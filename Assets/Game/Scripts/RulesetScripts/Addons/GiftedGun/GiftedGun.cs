using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftedGun : AddOn
{
    public List<string> possibleGunNames;
    public PickUpLoacation[] gunSpawns;
    public PickUp[] gunThatSpawnFromGunSpawns;
    PickUp[][] previousGunSpawns;
    PlayerManager[] allPlayers;

    PhotonView pv;

    void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    public override void StartAddOn()
    {
        previousGunSpawns = new PickUp[gunSpawns.Length][];
        allPlayers = PlayerWrangler.GetAllPlayers();

        foreach (PlayerManager player in allPlayers)
        {
            if (player != null)
            {
                player.PhotonView.RPC("RPC_Disarm", PhotonTargets.All);
                player.PhotonView.RPC("RPC_WeaponPickedUp", PhotonTargets.All, possibleGunNames[Random.Range(0, possibleGunNames.Count)]);
            }
        }

        for (byte index = 0; index < gunSpawns.Length; index++)
        {
            ChangeGunSpawns(index, gunSpawns[index]);
        }
    }

    public override void EndAddOn()
    {
        for (byte index = 0; index < gunSpawns.Length; index++)
        {
            ReturnGunSpawns(index, gunSpawns[index]);
        }
    }

    void ChangeGunSpawns(byte index, PickUpLoacation gunSpawn)
    {
        if (PhotonNetwork.isMasterClient)
        {
            gunSpawn.photonView.RPC("RPC_DestoryItsPickup", PhotonTargets.OthersBuffered);
            Destroy(gunSpawn.activePickUp);
        }

        gunSpawn.SpawnSelectPickup(possibleGunNames[Random.Range(0, possibleGunNames.Count)]);
    }

    void ReturnGunSpawns(byte index, PickUpLoacation gunSpawn)
    {
        if (PhotonNetwork.isMasterClient)
        {
            gunSpawn.photonView.RPC("RPC_DestoryItsPickup", PhotonTargets.OthersBuffered);
            Destroy(gunSpawn.activePickUp);
        }

        gunSpawn.SpawnRandomPickup();
    }

    [PunRPC]
    void RPC_SetNewGunTypes(byte index)
    {
        Debug.LogError("Set the new guns of index: " + index);
        previousGunSpawns[index] = gunSpawns[index].pickUpTypes;
        gunSpawns[index].pickUpTypes = gunThatSpawnFromGunSpawns;
    }

    [PunRPC]
    void RPC_SetOldGunTypes(byte index)
    {
        Debug.LogError("Set the old guns of index: " + index);
        gunSpawns[index].pickUpTypes = previousGunSpawns[index];
    }
}
