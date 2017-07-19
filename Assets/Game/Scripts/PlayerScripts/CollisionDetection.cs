using UnityEngine;
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
    Material factionColor;

    IEnumerator Start()
    {
        health = GetComponentInParent<PlayerHealth>();                                                                    //References to the health and shooting scripts
    
        yield return new WaitForSeconds(1);
        playerManager.GetComponentInParent<PlayerManager>();
        faction = playerManager.GetFaction();
        factionColor = playerManager.GetFactionColor();
    }
    
    public void OnHit(short damage, string sourceID)                                                                                         //Gets called from the shooting scripts raycast, we use other to determine how much damage we will take.
    {
        if (health.isPlayerDead()) return;
        switch(collisionLocation)                                                                                         //Find the collisionLocation this collider is marked with.
        {
            case CollisionFlag.FrontHeadShot:
                if(playerManager.isServer)
                    health.RpcTookDamage((byte)(damage * 2), sourceID, CollisionFlag.FrontHeadShot);                                       
                else
                    health.CmdTookDamage((byte)(damage * 2), sourceID, CollisionFlag.FrontHeadShot);
                break;
            case CollisionFlag.BackHeadShot:
                if (playerManager.isServer)
                    health.RpcTookDamage((byte)(damage * 2), sourceID, CollisionFlag.BackHeadShot);
                else
                    health.RpcTookDamage((byte)(damage * 2), sourceID, CollisionFlag.BackHeadShot);
                break;
            case CollisionFlag.Front:
                if (playerManager.isServer)
                    health.RpcTookDamage(damage, sourceID, CollisionFlag.Front);
                else
                    health.RpcTookDamage(damage, sourceID, CollisionFlag.Front);
                break;
            case CollisionFlag.Back:
                if (playerManager.isServer)
                    health.RpcTookDamage(damage, sourceID, CollisionFlag.Back);
                else
                    health.RpcTookDamage(damage, sourceID, CollisionFlag.Back);
                break;
            case CollisionFlag.Left:
                if (playerManager.isServer)
                    health.RpcTookDamage(damage, sourceID, CollisionFlag.Left);
                else
                    health.RpcTookDamage(damage, sourceID, CollisionFlag.Left);
                break;
            case CollisionFlag.Right:
                if (playerManager.isServer)
                    health.RpcTookDamage(damage, sourceID, CollisionFlag.Right);
                else
                    health.RpcTookDamage(damage, sourceID, CollisionFlag.Right);
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag.Equals("Ball"))
        {
            other.GetComponent<Rigidbody>().AddForce(transform.forward * .75f, ForceMode.VelocityChange);
            other.GetComponent<Ball>().SetFaction(faction, factionColor);
        }
    }
}