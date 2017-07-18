using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Ball : NetworkBehaviour
{
    GameObject ballRespawn;
    string faction;
    Material factionColor;
    Renderer rend;

	void Start ()
    {
        rend = GetComponent<Renderer>();
        rend.material.color = Color.white;
        ballRespawn = GameObject.Find("BallSpawnpoint");
        NetworkServer.Spawn(gameObject);
	}

    void OnEnable()
    {
        rend = GetComponent<Renderer>();
        rend.material.color = Color.white;
        ballRespawn = GameObject.Find("BallSpawnpoint");
    }

    public void SetFaction(string myFaction, Material myColor)
    {
        faction = myFaction;
        factionColor = myColor;
        rend.material = factionColor;
    }

    public string GetFaction()
    {
        return faction;
    }

    public IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1);
        rend.material.color = Color.white;
        transform.position = ballRespawn.transform.position;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
