using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance;
    public Text timerText;
    public GameObject scorePanel;
    public GameObject scoreboardTextObj;
    //[HideInInspector]
    public List<Text> scoreboardTextObjList;

    public Dictionary<string, int> playerScores;

    [HideInInspector]
    public bool isActive;

    [SyncVar]
    int minutes;
    [SyncVar]
    int seconds;

    void Awake()
    {
        if (instance != null)
            Debug.LogError("More than one GameManager in scene.");
        else
            instance = this;

        playerScores = new Dictionary<string, int>();
    }

    void Start()
    {
        minutes = GameCustomization.gameLength;
        seconds = 1;
        StartTimer();
    }

    [ServerCallback]
    void StartTimer()
    {
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        for(int i = seconds; i >= 0; i--)
        {
            if (seconds == 0)
            {
                seconds = 60;
                i = seconds;
                minutes--;
            }            
            seconds--;
            yield return new WaitForSeconds(1);            
        }
    }

    public void AddPlayer(string playerID)
    {
        playerScores.Add(playerID, 0);
        GameObject temp = Instantiate(scoreboardTextObj, scorePanel.transform);
        scoreboardTextObjList.Add(temp.GetComponent<Text>());
    }

    public void ScoreBoard(bool _isActive)
    {
        UpdateScoreText();
        scorePanel.SetActive(_isActive);
        isActive = _isActive;
    }

    [ClientRpc]
    public void RpcAddScore(string player, int amount)
    {
        if (!playerScores.ContainsKey(player)) return;
        playerScores[player] += amount;
    }

    void Update()
    {
        UpdateTimerText();
    }

    [Client]
    void UpdateTimerText()
    {
        timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
    }

    [Client]
    void UpdateScoreText()
    {
        int count = 0;
        foreach(string name in playerScores.Keys)
        {
            scoreboardTextObjList[count].text = name + ": " + playerScores[name];
            count++;
        }
    }
}