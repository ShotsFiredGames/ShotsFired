using System.Collections;
using UnityEngine;

public class ControlPoint : Point
{
    short playersOnPoint;
    short pointsRecieved;
    double pointFreq;

    bool isGivingPoints;
    Color orginalColor;

    public void SetValues(short pRec, double pFreq)
    {
        pointsRecieved = pRec;
        pointFreq = pFreq;
        orginalColor = gameObject.GetComponent<Renderer>().material.color;
    }

    public override void PlayerEnters(GameObject player)
    {
        playersOnPoint++;

        if (playersOnPoint > 1)
        {
            gameObject.GetComponent<Renderer>().material.color = orginalColor;
            RefereeManager.instance.PlayCaptureContested();
        }
        else //someone entered the point and they are the only ones on it
        {
            if (player.name.Equals(PhotonNetwork.player.NickName))
                RefereeManager.instance.PlayCapturePoint();
        }
    }

    public override void PlayerStays(GameObject player)
    {
        if (playersOnPoint == 1)
        {
            if (!isGivingPoints)
                StartCoroutine(GivePointsOnPoint(player.name));

            string faction = "";

            if (GetComponent<Renderer>().material.color.Equals(orginalColor))
            {
                faction = player.GetComponent<PlayerManager>().GetFaction();
                gameObject.GetComponent<Renderer>().material.color = PlayerWrangler.GetFactionMaterial(faction).color;
            }           
        }
    }

    public override void PlayerLeaves(GameObject player)
    {
        playersOnPoint--;

        if (playersOnPoint == 0)
        {
            gameObject.GetComponent<Renderer>().material.color = orginalColor;
        }
        else if (playersOnPoint == 1)
        {
            if (player.name.Equals(PhotonNetwork.player.NickName))
                RefereeManager.instance.PlayCapturePoint();
        }
            
    }

    IEnumerator GivePointsOnPoint(string playerName)
    {
        isGivingPoints = true;
        yield return new WaitForSeconds((float)pointFreq);

        if (playersOnPoint == 1)
        {
            if (PhotonNetwork.player.NickName.Equals(playerName))
                GameManager.instance.PlayerGainsPoints(playerName, pointsRecieved);
        }            

        isGivingPoints = false;
    }
}