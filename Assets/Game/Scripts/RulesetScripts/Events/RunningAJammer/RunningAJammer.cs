using System.Collections.Generic;
using UnityEngine;

public class RunningAJammer : GameEvent
{
    public Jammer[] jammers;
    public GameObject[] objectsToSetActive;
    public Dictionary<string, int> playerJammerInfo;
    public byte numJammerForPoints = 5;
    public short pointsPerJammerSet;

    void Awake()
    {
        ActivateJammers(false);
    }

    public override void StartEvent()
    {
        ActivateJammers(true);

        foreach (Jammer jam in jammers)
        {
            jam.InitJammer();
            jam.SetConnection(this);
        }

        PlayerManager[] players = PlayerWrangler.GetAllPlayers();
        playerJammerInfo = new Dictionary<string, int>();

        foreach (PlayerManager player in players)
        {
            playerJammerInfo.Add(player.name, 0);
        }

        foreach (GameObject go in objectsToSetActive)
            go.SetActive(false);

        gameEventDur = StartCoroutine(EventDuration());
    }

    public override void EndEvent()
    {
        ActivateJammers(false);

        foreach (GameObject go in objectsToSetActive)
            go.SetActive(true);

        EventManager.currentEvent = null;
    }

    public void UpdatePlayerJammerInfo(string player)
    {
        if (!playerJammerInfo.ContainsKey(player)) return;

        playerJammerInfo[player]++;

        int numOfJammers = playerJammerInfo[player];

        if (numOfJammers % numJammerForPoints == 0)
        {
            GameManager.instance.PlayerGainsPoints(player, pointsPerJammerSet);
        }

        if (numOfJammers >= jammers.Length)
        {
            Debug.LogError(player + " has touched all the points");
            foreach (Jammer jam in jammers)
            {
                jam.ResetPoint(player);
            }

            playerJammerInfo[player] = 0;
            RefereeManager.instance.PlayJammersReset();
        }
    }

    void ActivateJammers(bool isActive)
    {
        foreach (Jammer jam in jammers)
        {
            jam.gameObject.SetActive(isActive);
        }
    }
}