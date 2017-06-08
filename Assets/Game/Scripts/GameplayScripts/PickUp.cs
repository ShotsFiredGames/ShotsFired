using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PickUp : MonoBehaviour
{
    public enum PickUpType
    {
        None,
        Gun,
        Ability
    };

    public enum GunType
    {
        None,
        MachineGun, 
        RocketLauncher
    };

    public enum AbilityType
    {
        None,
        SuperBoots,
        Juggernaut
    };

    PickUpManager pickUpManager;

    [HideInInspector]
    public PickUpType type = PickUpType.None;
    [HideInInspector]
    public GunType gun = GunType.None;
    [HideInInspector]
    public AbilityType ability = AbilityType.None;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Collision"))
        {
            pickUpManager = other.transform.root.GetComponent<PickUpManager>();
            switch(type)
            {
                case PickUpType.Gun:
                    Gun();
                    break;

                case PickUpType.Ability:
                    Ability();
                    break;
            }

            NetworkServer.UnSpawn(gameObject);
            Destroy(gameObject);
        }
    }

    void Gun()
    {
        switch(gun)
        {
            case GunType.None:
                break;
            case GunType.MachineGun:
                pickUpManager.ApplyPickUp("Gun", "MachineGun");
                break;
            case GunType.RocketLauncher:
                pickUpManager.ApplyPickUp("Gun", "RocketLauncher");
                break;
        }
    }

    void Ability()
    {
        switch (ability)
        {
            case AbilityType.None:
                break;
            case AbilityType.SuperBoots:
                pickUpManager.ApplyPickUp("Ability", "SuperBoots");
                break;
            case AbilityType.Juggernaut:
                pickUpManager.ApplyPickUp("Ability", "Juggernaut");
                break;
        }
    }
}
