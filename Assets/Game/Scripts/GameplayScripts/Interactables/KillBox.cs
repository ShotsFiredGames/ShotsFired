using UnityEngine;

public class KillBox : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            other.GetComponent<PlayerHealth>().Local_InstantDeath("Environment", CollisionDetection.CollisionFlag.Back);
            other.GetComponent<PlayerHealth>().PhotonView.RPC("RPC_InstantDeath", PhotonTargets.All, "Environment", CollisionDetection.CollisionFlag.Back);
        }
    }
}