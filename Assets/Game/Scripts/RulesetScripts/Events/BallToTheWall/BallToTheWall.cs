using System.Collections;
using UnityEngine;

public class BallToTheWall : GameEvent
{
    public short scoreAmount;

    [HideInInspector]
    public bool ballToTheWallActive;
    public GameObject[] objectsToSetActive;
    public GameObject[] objectsToSetInActive;
    public GameObject[] spawnpoints;
    public Collider[] goals;

    public GameObject ball;
    public GameObject ballRespawn;
    GameObject activeBall;

    void Awake()
    {
        PhotonView = GetComponent<PhotonView>();
    }
    
    private void Start()
    {
        if (PhotonNetwork.isMasterClient)
        {
            int _viewID = PhotonNetwork.AllocateViewID();
            PhotonView.RPC("RPC_SpawnBall", PhotonTargets.All, _viewID);
        }
        //activeBall = PhotonNetwork.Instantiate(ball.name, ballRespawn.transform.position, ballRespawn.transform.rotation, 0);
        //activeBall.transform.parent = transform;
        //activeBall.GetComponent<Ball>().SetVariables(this);
        //activeBall.SetActive(false);
    }

    [PunRPC]
    void RPC_SpawnBall(int _viewID)
    {
        activeBall = Instantiate(ball, ballRespawn.transform.position, ballRespawn.transform.rotation);
        activeBall.GetComponent<PhotonView>().viewID = _viewID;
        activeBall.transform.parent = transform;
        activeBall.GetComponent<Ball>().SetVariables(this);
        activeBall.SetActive(false);
    }

    public override void StartEvent()
    {
        ballToTheWallActive = true;
        activeBall.SetActive(true);

        for (int i = 0; i < objectsToSetInActive.Length; i++)
            objectsToSetInActive[i].SetActive(false);

        for (int i = 0; i < objectsToSetActive.Length; i++)
            objectsToSetActive[i].SetActive(true);

        for (int i = 0; i < PlayerWrangler.GetAllPlayers().Length; i++)
        {
            PlayerWrangler.GetAllPlayers()[i].transform.position = spawnpoints[i].transform.position;
            PlayerWrangler.GetAllPlayers()[i].transform.rotation = spawnpoints[i].transform.rotation;
            PlayerWrangler.GetAllPlayers()[i].RPC_Disarm();
        }

        for (int i = 0; i < goals.Length; i++)
            goals[i].enabled = true;

        gameEventDur = StartCoroutine(EventDuration());
    }

    public override void EndEvent()
    {
        for (int i = 0; i < goals.Length; i++)
            goals[i].enabled = false;

        for (int i = 0; i < objectsToSetActive.Length; i++)
            objectsToSetActive[i].SetActive(false);

        for (int i = 0; i < objectsToSetInActive.Length; i++)
            objectsToSetInActive[i].SetActive(true);

        activeBall.SetActive(false);
        ballToTheWallActive = false;
    }

    public void PlayerScored(string player)
    {
        if (GameManager.instance.PhotonView != null && PhotonNetwork.isMasterClient)
            GameManager.instance.PhotonView.RPC("RPC_AddScore", PhotonTargets.All, player, scoreAmount);
    }

    public void RespawnBall()
    {
        //if(PhotonNetwork.isMasterClient)
        Debug.LogError("THIS SHOULD RESET THE POSITION");
        activeBall.transform.position = ballRespawn.transform.position;
    }
}