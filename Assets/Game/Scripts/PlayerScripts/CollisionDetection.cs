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
				health.Local_TookDamage ((short)(damage * 2), sourceID, CollisionFlag.FrontHeadShot);
				photonView.RPC("RPC_TookDamage", PhotonTargets.Others, (short)(damage * 2), sourceID, CollisionFlag.FrontHeadShot);      //Tell our health script how much damage we took from the enemies shooting script and the location we were hit from.
	            break;
	        case CollisionFlag.BackHeadShot:
				health.Local_TookDamage ((short)(damage * 2), sourceID, CollisionFlag.BackHeadShot);
				photonView.RPC("RPC_TookDamage", PhotonTargets.Others, (short)(damage * 2), sourceID, CollisionFlag.BackHeadShot);
	            break;
	        case CollisionFlag.Front:
				health.Local_TookDamage (damage, sourceID, CollisionFlag.Front);
				photonView.RPC("RPC_TookDamage", PhotonTargets.Others, damage, sourceID, CollisionFlag.Front);
	            break;
	        case CollisionFlag.Back:
				health.Local_TookDamage (damage, sourceID, CollisionFlag.Back);
				photonView.RPC("RPC_TookDamage", PhotonTargets.Others, damage, sourceID, CollisionFlag.Back);
	            break;
	        case CollisionFlag.Left:
				health.Local_TookDamage (damage, sourceID, CollisionFlag.Left);
				photonView.RPC("RPC_TookDamage", PhotonTargets.Others, damage, sourceID, CollisionFlag.Left);
	            break;
	        case CollisionFlag.Right:
				health.Local_TookDamage (damage, sourceID, CollisionFlag.Right);
				photonView.RPC("RPC_TookDamage", PhotonTargets.Others, damage, sourceID, CollisionFlag.Right);
	            break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag.Equals("Ball"))
        {
            other.GetComponent<Rigidbody>().AddForce(transform.forward * .75f, ForceMode.VelocityChange);
			Ball tempBall = other.GetComponent<Ball> ();
			tempBall.Local_SetFactionBall (faction);
			tempBall.PhotonView.RPC("RPC_SetFactionBall", PhotonTargets.Others, faction);
        }
    }
}