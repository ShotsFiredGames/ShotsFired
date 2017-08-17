using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedHealing : AddOn
{
    public HealingPad[] pads;

    public override void StartAddOn()
    {
        StartCoroutine(ActivateHealingPads());
    }

    IEnumerator ActivateHealingPads()
    {
        foreach (HealingPad pad in pads)
        {
            pad.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(GameCustomization.eventOccurenceRate / 3);

        foreach (HealingPad pad in pads)
        {
            pad.gameObject.SetActive(false);
        }
    }
}
