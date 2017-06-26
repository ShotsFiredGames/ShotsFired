using UnityEngine;
using UnityEngine.Networking;

public class KillBox : NetworkBehaviour {

	[ServerCallback]
    void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Player"))
        {
            other.GetComponent<PlayerHealth>().RpcInstantDeath("Environment", CollisionDetection.CollisionFlag.Back);
        }
    }
}
