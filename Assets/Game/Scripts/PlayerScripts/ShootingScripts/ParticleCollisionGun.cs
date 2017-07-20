using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollisionGun : Gun
{
    private void Start()
    {
        SetAmmo();
    }

    public override void Discard()
    {
        playerManager.CmdDisarm();
    }
}
