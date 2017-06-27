﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RocketExplosion : NetworkBehaviour
{
    public byte damage;
    string playername;

    [ServerCallback]
    void OnTriggerEnter(Collider other)
    {
        if (gameObject.activeInHierarchy && other.transform.root.name != playername)
        {
            if (other.tag.Equals("Collision"))
            {
                other.GetComponent<CollisionDetection>().OnHit(damage, playername);
            }
        }
    }

    public void SetVariables(string _playername)
    {
        playername = _playername;
    }
}
