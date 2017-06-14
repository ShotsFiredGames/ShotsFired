using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PickUpManager : NetworkBehaviour
{
    PlayerManager playerManager;
    public AudioSource pickupSource;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
    }

    [ClientRpc]
    public void RpcApplyPickUp(string pickUpType, string pickUp)
    {
        switch(pickUpType)
        {
            case "Gun":
                playerManager.CmdWeaponPickedUp(pickUp);
                break;
            case "Ability":
                playerManager.CmdAbilityPickedUp(pickUp);
                break;
        }
    }

    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("PickUp"))
        {
            PickUp pickup = other.GetComponent<PickUp>();
            pickupSource.PlayOneShot(pickup.pickupSound);
            switch (pickup.type)
            {
                case PickUp.PickUpType.Gun:
                    Gun(pickup);
                    break;

                case PickUp.PickUpType.Ability:
                    Ability(pickup);
                    break;
            }

            NetworkServer.UnSpawn(other.gameObject);
            Destroy(other.gameObject);
        }
    }


    public void Gun(PickUp pickup)
    {
        switch (pickup.gun)
        {
            case PickUp.GunType.None:
                break;
            case PickUp.GunType.MachineGun:
                RpcApplyPickUp("Gun", "MachineGun");
                break;
            case PickUp.GunType.RocketLauncher:
                RpcApplyPickUp("Gun", "RocketLauncher");
                break;
        }
    }

    public void Ability(PickUp pickup)
    {
        switch (pickup.ability)
        {
            case PickUp.AbilityType.None:
                break;
            case PickUp.AbilityType.SuperBoots:
                RpcApplyPickUp("Ability", "SuperBoots");
                break;
            case PickUp.AbilityType.Juggernaut:
                RpcApplyPickUp("Ability", "Juggernaut");
                break;
        }
    }
}
