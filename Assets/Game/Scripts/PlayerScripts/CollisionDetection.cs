using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

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
                health.CmdTookDamage(damage * 2, sourceID, CollisionFlag.FrontHeadShot);                                        //Tell our health script how much damage we took from the enemies shooting script and the location we were hit from.
                break;
            case CollisionFlag.BackHeadShot:
                health.CmdTookDamage(damage * 2, sourceID, CollisionFlag.BackHeadShot);
                break;
            case CollisionFlag.Front:
                health.CmdTookDamage(damage, sourceID, CollisionFlag.Front);
                break;
            case CollisionFlag.Back:
                health.CmdTookDamage(damage, sourceID, CollisionFlag.Back);
                break;
            case CollisionFlag.Left:
                health.CmdTookDamage(damage, sourceID, CollisionFlag.Left);
                break;
            case CollisionFlag.Right:
                health.CmdTookDamage(damage, sourceID, CollisionFlag.Right);
                break;
        }
    }
}