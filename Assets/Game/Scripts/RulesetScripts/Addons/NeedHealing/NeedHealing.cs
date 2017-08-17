using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedHealing : AddOn
{
    public byte healthAmount;
    public float healFreq;
    public HealingPad[] pads;

    void Start()
    {
        ActivateAllPads(false);

        foreach (HealingPad pad in pads)
        {
            pad.SetValues(healthAmount, healFreq);
        }

    }

    public override void StartAddOn()
    {
        StartCoroutine(ActivateHealingPads());
    }

    IEnumerator ActivateHealingPads()
    {
        ActivateAllPads(true);

        yield return new WaitForSeconds(GameCustomization.eventOccurenceRate / 3);

        ActivateAllPads(false);
    }

    void ActivateAllPads(bool isActive)
    {
        foreach (HealingPad pad in pads)
        {
            pad.gameObject.SetActive(isActive);
        }
    }
}
