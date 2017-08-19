﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftedGun : AddOn
{
    public List<string> possibleGunNames;
    public PickUpLoacation[] gunSpawns;
    PlayerManager[] allPlayers;

    PhotonView pv;

    void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    public override void StartAddOn()
    {
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
            ChangeGunSpawns(gunSpawns[index]);
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
            gunSpawn.photonView.RPC("RPC_DestoryItsPickup", PhotonTargets.OthersBuffered);
            Destroy(gunSpawn.activePickUp);
        }

        gunSpawn.SpawnSelectPickup(possibleGunNames[Random.Range(0, possibleGunNames.Count)]);
    }

    void ReturnGunSpawns(PickUpLoacation gunSpawn)
    {
        if (PhotonNetwork.isMasterClient)
        {
            gunSpawn.photonView.RPC("RPC_DestoryItsPickup", PhotonTargets.OthersBuffered);
            Destroy(gunSpawn.activePickUp);
        }

        gunSpawn.SpawnRandomPickup();
    }
}
