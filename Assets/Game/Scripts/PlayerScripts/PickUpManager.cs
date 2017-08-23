using UnityEngine;

public class PickUpManager : Photon.MonoBehaviour
{
    PlayerManager playerManager;
    public AudioSource pickupSource;

    public AudioClip juggernautSound;
    public AudioClip overchargedSound;

    public AudioClip gunPickUpSound;

    PhotonView pv;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
    }

	public void Local_ApplyPickUp(string pickUpType, string pickUp)
	{
		switch (pickUpType)
		{
			case "Gun":
				pickupSource.PlayOneShot (gunPickUpSound);
				playerManager.Local_WeaponPickedUp (pickUp);
				playerManager.PhotonView.RPC("RPC_WeaponPickedUp", PhotonTargets.Others, pickUp);
				break;
			case "Ability":
				switch (pickUp) {
					case "Juggernaut":
						pickupSource.PlayOneShot (juggernautSound);
						break;
					case "Overcharged":
						pickupSource.PlayOneShot (overchargedSound);
						break;
					default:
						pickupSource.PlayOneShot (juggernautSound);
						break;
				}
				playerManager.Local_AbilityPickedUp (pickUp);
				playerManager.PhotonView.RPC("RPC_AbilityPickedUp", PhotonTargets.Others, pickUp);
				break;
		}
	}

    [PunRPC]
    public void RPC_ApplyPickUp(string pickUpType, string pickUp)
    {
        switch (pickUpType)
        {
            case "Gun":
                pickupSource.PlayOneShot(gunPickUpSound);
				playerManager.Local_WeaponPickedUp (pickUp);
                //playerManager.PhotonView.RPC("RPC_WeaponPickedUp", PhotonTargets.All, pickUp);
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
				playerManager.Local_AbilityPickedUp (pickUp);
                //playerManager.PhotonView.RPC("RPC_AbilityPickedUp", PhotonTargets.All, pickUp);
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
            {
                pv = other.GetComponent<PhotonView>();
                if (pv)
                {
                    Debug.LogError("destroy pickup");
                    photonView.RPC("RPC_DestroyPickup", PhotonTargets.Others, pv.viewID);
                    Destroy(other.gameObject);
                }
            }
        }

//        if (other.tag.Equals("Mimic"))
//        {
//            photonView.RPC("RpcActivateExplosion", PhotonTargets.All, other.gameObject.transform.position);
//            if (PhotonNetwork.isMasterClient)
//                PhotonNetwork.Destroy(other.GetComponent<PhotonView>());
//            photonView.RPC("RPC_InstantDeath", PhotonTargets.All, "Mimic", CollisionDetection.CollisionFlag.Front);
//        }

//        if (other.tag.Equals("SpeedBoost"))
//        {
//            playerManager.EnteredSpeedBoost();
//        }
    }

//    private void OnTriggerExit(Collider other)
//    {
//        if (other.tag.Equals("SpeedBoost"))
//        {
//            playerManager.SpeedBoost();
//        }
//    }

    public void Gun(PickUp pickup)
    {
        switch (pickup.gun)
        {
	        case PickUp.GunType.None:
	            break;
			case PickUp.GunType.MachineGun:
				Local_ApplyPickUp ("Gun", "MachineGun");
				playerManager.PhotonView.RPC("RPC_ApplyPickUp", PhotonTargets.Others, "Gun", "MachineGun");
	            break;
	        case PickUp.GunType.RocketLauncher:
				Local_ApplyPickUp ("Gun", "RocketLauncher");
				playerManager.PhotonView.RPC("RPC_ApplyPickUp", PhotonTargets.Others, "Gun", "RocketLauncher");
	            break;
	        case PickUp.GunType.ShotGun:
				Local_ApplyPickUp ("Gun", "ShotGun");
				playerManager.PhotonView.RPC("RPC_ApplyPickUp", PhotonTargets.Others, "Gun", "ShotGun");
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
				Local_ApplyPickUp ("Ability", "Juggernaut");
				playerManager.PhotonView.RPC("RPC_ApplyPickUp", PhotonTargets.Others, "Ability", "Juggernaut");
	            break;
	        case PickUp.AbilityType.Overcharged:
				Local_ApplyPickUp ("Ability", "Overcharged");
				playerManager.PhotonView.RPC("RPC_ApplyPickUp", PhotonTargets.Others, "Ability", "Overcharged");
	            break;
        }
    }

    [PunRPC]
    void RPC_DestroyPickup(int viewID)
    {
        if (PhotonView.Find(viewID))
            Destroy(PhotonView.Find(viewID).gameObject);
    }
}