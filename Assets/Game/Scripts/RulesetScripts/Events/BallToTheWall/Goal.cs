using UnityEngine;

public class Goal : MonoBehaviour
{
    BallToTheWall ballToTheWall;

    private void Start()
    {
        ballToTheWall = GameObject.Find("BallToTheWall").GetComponent<BallToTheWall>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Ball"))
        {
            for(int i = 0; i < PlayerWrangler.GetAllPlayers().Length; i++)
            {
                if(PlayerWrangler.GetAllPlayers()[i].GetFaction() == other.GetComponent<Ball>().GetFaction())
                {
                    ballToTheWall.PlayerScored(PlayerWrangler.GetAllPlayers()[i].name);
                    StartCoroutine(other.GetComponent<Ball>().Respawn());
                }
            }
        }
    }
}
