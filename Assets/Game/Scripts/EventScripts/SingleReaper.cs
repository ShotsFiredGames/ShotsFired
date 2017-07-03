using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SingleReaper : AddOn
{
    public Reaper reaper;
    public Transform spawnPoint;
    [Tooltip("This number is subtracted. Make it positive if you want the player to lose points")]
    public byte pointsPlayerLosesOnDeath;

    [ServerCallback]
    private void Start()
    {
        NetworkServer.Spawn(reaper.gameObject);
    }

    public override void StartAddOn()
    {
        reaper.enabled = true;
        reaper.canDie = false;
        reaper.SetTargetPlayer(PlayerWrangler.GetPlayer(GameManager.instance.GetWinningPlayer()));
        reaper.SetSpawnPoint(spawnPoint);
        reaper.SetPoints(pointsPlayerLosesOnDeath);
        reaper.Setup();
    }
}
