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
        playerManager = GetComponentInParent<PlayerManager>();
        yield return new WaitForSeconds(1);
        faction = playerManager.GetFaction();
        factionColor = playerManager.GetFactionColor();
    }
    
    public void OnHit(short damage, string sourceID)                                                                                         //Gets called from the shooting scripts raycast, we use other to determine how much damage we will take.
    {
        if (health.isPlayerDead()) return;
        switch(collisionLocation)                                                                                         //Find the collisionLocation this collider is marked with.
        {
            case CollisionFlag.FrontHeadShot:
                    health.RpcTookDamage((byte)(damage * 2), sourceID, CollisionFlag.FrontHeadShot);
                break;
            case CollisionFlag.BackHeadShot:
                    health.RpcTookDamage((byte)(damage * 2), sourceID, CollisionFlag.BackHeadShot);
                break;
            case CollisionFlag.Front:
                    health.RpcTookDamage(damage, sourceID, CollisionFlag.Front);
                break;
            case CollisionFlag.Back:
                    health.RpcTookDamage(damage, sourceID, CollisionFlag.Back);
                break;
            case CollisionFlag.Left:
                    health.RpcTookDamage(damage, sourceID, CollisionFlag.Left);
                break;
            case CollisionFlag.Right:
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