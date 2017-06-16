﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketExplosion : MonoBehaviour
{
    public int damage;
    string playername;

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.name != playername)
        {
            if (other.tag.Equals("Collision"))
            {
                Debug.LogError("Delt explsion damage");
                other.GetComponent<CollisionDetection>().OnHit(damage, playername);
            }
        }
    }

    public void SetVariables(string _playername)
    {
        playername = _playername;
    }
}
