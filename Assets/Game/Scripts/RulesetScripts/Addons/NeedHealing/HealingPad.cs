﻿using System.Collections;
using UnityEngine;

public class HealingPad : Point
{
    byte healthAmount;
    float healFreq;

    public void SetValues(byte health, float freq)
    {
        healthAmount = health;
        healFreq = freq;
    }

    public override void PlayerStays(GameObject player)
    {
        PlayerHealth health = player.GetComponent<PlayerHealth>();

        if (health != null)
        {
            if (!health.isBeingHealed)
            {
                StartCoroutine(RestoreHealth(health));
            }
        }
    }

    IEnumerator RestoreHealth(PlayerHealth health)
    {
        health.isBeingHealed = true;
        health.Local_RestoreHealth(healthAmount);
        health.PhotonView.RPC("RPC_RestoreHealth", PhotonTargets.Others, healthAmount);
        yield return new WaitForSeconds(healFreq);
        health.isBeingHealed = false;
    }
}
