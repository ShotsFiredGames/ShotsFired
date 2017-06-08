using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpManager : MonoBehaviour
{
    PlayerManager playerManager;

    private void Start()
    {
        playerManager = GetComponent<PlayerManager>();
    }
    public void ApplyPickUp(string pickUpType, string pickUp)
    {
        switch(pickUpType)
        {
            case "Gun":
                playerManager.WeaponPickedUp(pickUp);
                break;

            case "Ability":
                playerManager.AbilityPickedUp(pickUp);
                break;
        }
    }
}
