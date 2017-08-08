using System.Collections.Generic;
using UnityEngine;

public class RocketExplosion : MonoBehaviour
{
    public byte damage;
    string playername;
    List<string> players = new List<string>();
    
    void OnTriggerEnter(Collider other)
    {
        //Debug.LogError("playername: " + playername);
        //Debug.LogError(other.transform.root.name);
        if (gameObject.activeInHierarchy && other.transform.root.name != playername)
        {
            if (other.tag.Equals("Collision"))
            {
                if (!players.Contains(other.transform.root.name))
                {
                    players.Add(other.transform.root.name);
                    other.GetComponent<CollisionDetection>().OnHit(damage, playername);
                }
            }
        }
    }

    private void OnDisable()
    {
        players.Clear();
    }

    [PunRPC]
    public void RPC_SetExplosionVariables(string _playername)
    {
        playername = _playername;
    }
}