using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ProjectileGun : Gun
{
    public override void Discard()
    {
        playerManager.CmdDisarm();
    }
}
