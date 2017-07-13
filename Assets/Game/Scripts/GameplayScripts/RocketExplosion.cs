using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RocketExplosion : NetworkBehaviour
{
    public byte damage;
    string playername;
    List<string> players = new List<string>();

    [ServerCallback]
    void OnTriggerEnter(Collider other)
    {
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

    public void SetVariables(string _playername)
    {
        playername = _playername;
    }
}
