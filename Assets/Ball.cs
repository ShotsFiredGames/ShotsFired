using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Ball : NetworkBehaviour
{
    string faction;
    Material factionColor;
    Renderer rend;
    BallToTheWall ballToTheWall;

	void Start ()
    {
        rend = GetComponent<Renderer>();
        rend.material.color = Color.white;
	}

    void OnEnable()
    {
        rend = GetComponent<Renderer>();
        rend.material.color = Color.white;
    }

    public void SetVariables(BallToTheWall _ballToTheWall)
    {
        ballToTheWall = _ballToTheWall;
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
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        ballToTheWall.RespawnBall();
    }
}
