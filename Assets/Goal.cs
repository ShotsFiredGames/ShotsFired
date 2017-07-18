using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public short scoreAmount;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Ball"))
        {
            print("Collided with ball");
            for(int i = 0; i < PlayerWrangler.GetAllPlayers().Length; i++)
            {
                print("Player Factions: " + PlayerWrangler.GetAllPlayers()[i].GetFaction());
                print("Ball Faction: " + other.GetComponent<Ball>().GetFaction());
                if(PlayerWrangler.GetAllPlayers()[i].GetFaction() == other.GetComponent<Ball>().GetFaction())
                {
                    print("Same Faction");
                    GameManager.instance.CmdAddScore(PlayerWrangler.GetAllPlayers()[i].name, scoreAmount);
                    StartCoroutine(other.GetComponent<Ball>().Respawn());
                }
            }
        }
    }
}
