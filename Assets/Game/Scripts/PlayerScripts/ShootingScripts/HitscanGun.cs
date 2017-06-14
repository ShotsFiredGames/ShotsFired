using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HitscanGun : Gun
{
    public LayerMask layermask;

    RaycastHit hit;
    Shooting shooting;

    void Start()
    {
        SetAmmo();
        shooting = transform.root.GetComponent<Shooting>();
    }

    public override void Discard()
    {
        playerManager.CmdDisarm();
    }
}
