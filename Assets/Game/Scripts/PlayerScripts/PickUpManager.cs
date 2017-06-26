using UnityEngine;
using UnityEngine.Networking;

public class PickUpManager : NetworkBehaviour
{
    PlayerManager playerManager;
    public AudioSource pickupSource;

    public AudioClip juggernautSound;
    public AudioClip overchargedSound;

    public AudioClip gunPickUpSound;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
    }

    [ClientRpc]
    public void RpcApplyPickUp(string pickUpType, string pickUp)
    {
        switch (pickUpType)
        {
            case "Gun":
                pickupSource.PlayOneShot(gunPickUpSound);
                playerManager.CmdWeaponPickedUp(pickUp);
                break;
            case "Ability":
                switch (pickUp)
                {
                    case "Juggernaut":
                        pickupSource.PlayOneShot(juggernautSound);
                        break;
                    case "Overcharged":
                        pickupSource.PlayOneShot(overchargedSound);
                        break;
                    default:
                        pickupSource.PlayOneShot(juggernautSound);
                        break;
                }
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
            if(pickup.pickUpLocation != null)
                pickup.pickUpLocation.SetBool("HasAbility", false);

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
            case PickUp.AbilityType.Juggernaut:
                RpcApplyPickUp("Ability", "Juggernaut");
                break;
            case PickUp.AbilityType.Overcharged:
                RpcApplyPickUp("Ability", "Overcharged");
                break;
        }
    }
}
