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
    PhotonView photonView;
    string faction;

    IEnumerator Start()
    {
        health = GetComponentInParent<PlayerHealth>();                                                                    //References to the health and shooting scripts
        playerManager = GetComponentInParent<PlayerManager>();
        photonView = GetComponentInParent<PhotonView>();
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
                photonView.RPC("RPC_TookDamage", PhotonTargets.All, (short)(damage * 2), sourceID, CollisionFlag.FrontHeadShot);      //Tell our health script how much damage we took from the enemies shooting script and the location we were hit from.
                break;
            case CollisionFlag.BackHeadShot:
                photonView.RPC("RPC_TookDamage", PhotonTargets.All, (short)(damage * 2), sourceID, CollisionFlag.FrontHeadShot);
                break;
            case CollisionFlag.Front:
                photonView.RPC("RPC_TookDamage", PhotonTargets.All, damage, sourceID, CollisionFlag.FrontHeadShot);
                break;
            case CollisionFlag.Back:
                photonView.RPC("RPC_TookDamage", PhotonTargets.All, damage, sourceID, CollisionFlag.FrontHeadShot);
                break;
            case CollisionFlag.Left:
                photonView.RPC("RPC_TookDamage", PhotonTargets.All, damage, sourceID, CollisionFlag.FrontHeadShot);
                break;
            case CollisionFlag.Right:
                photonView.RPC("RPC_TookDamage", PhotonTargets.All, damage, sourceID, CollisionFlag.FrontHeadShot);
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag.Equals("Ball"))
        {
            other.GetComponent<Rigidbody>().AddForce(transform.forward * .75f, ForceMode.VelocityChange);
            other.GetComponent<Ball>().PhotonView.RPC("RPC_SetFaction", PhotonTargets.All, faction);
        }
    }
}