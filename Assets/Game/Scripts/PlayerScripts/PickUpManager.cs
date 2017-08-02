using UnityEngine;

public class PickUpManager : Photon.MonoBehaviour
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

    [PunRPC]
    public void RPC_ApplyPickUp(string pickUpType, string pickUp)
    {
        switch (pickUpType)
        {
            case "Gun":
                pickupSource.PlayOneShot(gunPickUpSound);
                playerManager.PhotonView.RPC("RPC_WeaponPickedUp", PhotonTargets.All, pickUp);
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
                playerManager.PhotonView.RPC("RPC_AbilityPickedUp", PhotonTargets.All, pickUp);
                break;
        }
    }

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

            if (PhotonNetwork.isMasterClient)
                PhotonNetwork.Destroy(other.GetComponent<PhotonView>());
        }

        if (other.tag.Equals("Mimic"))
        {
            photonView.RPC("RpcActivateExplosion", PhotonTargets.All, other.gameObject.transform.position);
            if (PhotonNetwork.isMasterClient)
                PhotonNetwork.Destroy(other.GetComponent<PhotonView>());
            photonView.RPC("RPC_InstantDeath", PhotonTargets.All, "Mimic", CollisionDetection.CollisionFlag.Front);
        }

        if (other.tag.Equals("SpeedBoost"))
        {
            playerManager.EnteredSpeedBoost();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("SpeedBoost"))
        {
            playerManager.SpeedBoost();
        }
    }

    public void Gun(PickUp pickup)
    {
        switch (pickup.gun)
        {
            case PickUp.GunType.None:
                break;
            case PickUp.GunType.MachineGun:
                playerManager.PhotonView.RPC("RPC_ApplyPickUp", PhotonTargets.All, "Gun", "MachineGun");
                break;
            case PickUp.GunType.RocketLauncher:
                playerManager.PhotonView.RPC("RPC_ApplyPickUp", PhotonTargets.All, "Gun", "RocketLauncher");
                break;
            case PickUp.GunType.ShotGun:
                playerManager.PhotonView.RPC("RPC_ApplyPickUp", PhotonTargets.All, "Gun", "ShotGun");
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
                playerManager.PhotonView.RPC("RPC_ApplyPickUp", PhotonTargets.All, "Ability", "Juggernaut");
                break;
            case PickUp.AbilityType.Overcharged:
                playerManager.PhotonView.RPC("RPC_ApplyPickUp", PhotonTargets.All, "Ability", "Overcharged");
                break;
        }
    }
}
