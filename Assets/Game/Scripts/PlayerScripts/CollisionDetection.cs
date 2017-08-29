﻿using UnityEngine;
using System.Collections;

public class CollisionDetection : MonoBehaviour
{
    public enum CollisionFlag                                                                                              //List of possible collision locations
    {
        FrontHeadShot,
        BackHeadShot,
        Front,
        Back,
        Left,
        Right
    };

    public CollisionFlag collisionLocation = CollisionFlag.FrontHeadShot;

    PlayerHealth health;
    PlayerManager playerManager;
    string faction;

    IEnumerator Start()
    {
        health = GetComponentInParent<PlayerHealth>();                                                                    //References to the health and shooting scripts
        playerManager = GetComponentInParent<PlayerManager>();
        yield return new WaitForSeconds(1);
        faction = playerManager.GetFaction();
    }
    
    public void OnHit(short damage, string sourceID)                                                                                         //Gets called from the shooting scripts raycast, we use other to determine how much damage we will take.
    {
        if (sourceID == transform.root.name) return;
        if (health.isPlayerDead()) return;
        switch(collisionLocation)                                                                                         //Find the collisionLocation this collider is marked with.
        {
            case CollisionFlag.FrontHeadShot:
                health.Local_TookDamage((short)(damage * 2), sourceID, CollisionFlag.FrontHeadShot);
                health.PhotonView.RPC("RPC_TookDamage", PhotonTargets.Others, (short)(damage * 2), sourceID, CollisionFlag.FrontHeadShot);      //Tell our health script how much damage we took from the enemies shooting script and the location we were hit from.
                break;
            case CollisionFlag.BackHeadShot:
                health.Local_TookDamage((short)(damage * 2), sourceID, CollisionFlag.BackHeadShot);
                health.PhotonView.RPC("RPC_TookDamage", PhotonTargets.Others, (short)(damage * 2), sourceID, CollisionFlag.BackHeadShot);
                break;
            case CollisionFlag.Front:
                health.Local_TookDamage(damage, sourceID, CollisionFlag.Front);
                health.PhotonView.RPC("RPC_TookDamage", PhotonTargets.Others, damage, sourceID, CollisionFlag.Front);
                break;
            case CollisionFlag.Back:
                health.Local_TookDamage(damage, sourceID, CollisionFlag.Back);
                health.PhotonView.RPC("RPC_TookDamage", PhotonTargets.Others, damage, sourceID, CollisionFlag.Back);
                break;
            case CollisionFlag.Left:
                health.Local_TookDamage(damage, sourceID, CollisionFlag.Left);
                health.PhotonView.RPC("RPC_TookDamage", PhotonTargets.Others, damage, sourceID, CollisionFlag.Left);
                break;
            case CollisionFlag.Right:
                health.Local_TookDamage(damage, sourceID, CollisionFlag.Right);
                health.PhotonView.RPC("RPC_TookDamage", PhotonTargets.Others, damage, sourceID, CollisionFlag.Right);
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag.Equals("Ball"))
        {
            other.GetComponent<Rigidbody>().AddForce(transform.forward * .75f, ForceMode.VelocityChange);
            other.GetComponent<Ball>().PhotonView.RPC("RPC_SetFactionBall", PhotonTargets.All, faction);
        }
    }
}