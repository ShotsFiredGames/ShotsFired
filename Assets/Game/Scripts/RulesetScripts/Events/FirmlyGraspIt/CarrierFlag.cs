using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrierFlag : Flag
{
    public float timeIncrement;

    bool isCoroutineRunning;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            if (!isPickedUp)
            {
                if (other.GetComponent<PlayerManager>().CheckAbilityToPickupFlag())
                {
                    isPickedUp = true;
					FlagManager.instance.Local_FlagPickedUp (index, other.transform.root.name);
					FlagManager.instance.photonView.RPC("RPC_FlagPickedUp", PhotonTargets.Others, index, other.transform.root.name);
                }
            }
        }
    }

    void Update()
    {
        if (carrier != null)
        {
            if (carrier.hasFlag)
            {
                if (!isCoroutineRunning)
                    StartCoroutine(HoldingFlag());
            }
        }
    }

    IEnumerator HoldingFlag()
    {
        isCoroutineRunning = true;
        yield return new WaitForSeconds(timeIncrement);
        Debug.LogError("Give points");
        FlagManager.instance.FlagHeld(carrier.name);
        isCoroutineRunning = false;
    }
}
