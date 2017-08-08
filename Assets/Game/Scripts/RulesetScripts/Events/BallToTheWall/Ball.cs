using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public PhotonView PhotonView { get; private set; }
    string faction;
    Material factionColor;
    Renderer rend;
    BallToTheWall ballToTheWall;

    void Awake()
    {
        PhotonView = GetComponent<PhotonView>();
    }

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

    [PunRPC]
    public void RPC_SetFaction(string myFaction)
    {
        faction = myFaction;
        rend.material = PlayerWrangler.GetFactionMaterial(faction);
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