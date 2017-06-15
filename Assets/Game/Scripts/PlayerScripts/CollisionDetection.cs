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

    void Start()
    {
        health = GetComponentInParent<PlayerHealth>();                                                                    //References to the health and shooting scripts
    }
    
    public void OnHit(int damage, string sourceID)                                                                                         //Gets called from the shooting scripts raycast, we use other to determine how much damage we will take.
    {
        switch(collisionLocation)                                                                                         //Find the collisionLocation this collider is marked with.
        {
            case CollisionFlag.FrontHeadShot:
                health.RpcHeadshotDamage(damage * 2, sourceID, CollisionFlag.FrontHeadShot);                                        //Tell our health script how much damage we took from the enemies shooting script and the location we were hit from.
                break;
            case CollisionFlag.BackHeadShot:
                health.RpcHeadshotDamage(damage * 2, sourceID, CollisionFlag.BackHeadShot);
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
}