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
            print("Hit a player");
            if (!isPickedUp)
            {
                if (!other.GetComponent<PlayerManager>().hasFlag)
                {
                    isPickedUp = true;
                    FlagManager.instance.CmdFlagPickedUp(index, other.transform.root.name);
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
