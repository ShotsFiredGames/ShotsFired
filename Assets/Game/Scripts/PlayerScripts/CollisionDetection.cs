using UnityEngine;

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
    
    public void OnHit(byte damage, string sourceID)                                                                                         //Gets called from the shooting scripts raycast, we use other to determine how much damage we will take.
    {
        switch(collisionLocation)                                                                                         //Find the collisionLocation this collider is marked with.
        {
            case CollisionFlag.FrontHeadShot:
                health.TookDamage((byte)(damage * 2), sourceID, CollisionFlag.FrontHeadShot);                                        //Tell our health script how much damage we took from the enemies shooting script and the location we were hit from.
                break;
            case CollisionFlag.BackHeadShot:
                health.TookDamage((byte)(damage * 2), sourceID, CollisionFlag.BackHeadShot);
                break;
            case CollisionFlag.Front:
                health.TookDamage(damage, sourceID, CollisionFlag.Front);
                break;
            case CollisionFlag.Back:
                health.TookDamage(damage, sourceID, CollisionFlag.Back);
                break;
            case CollisionFlag.Left:
                health.TookDamage(damage, sourceID, CollisionFlag.Left);
                break;
            case CollisionFlag.Right:
                health.TookDamage(damage, sourceID, CollisionFlag.Right);
                break;
        }
    }
}