using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance;
    public GameObject originalSpawnPoints;
    public GameObject newSpawnPoints;
    public Text timerText;
    public Text endGameText;
    public GameObject scorePanel;
    public GameObject scoreboardTextObj;
    [HideInInspector]
    public List<Text> scoreboardTextObjList;

    public Dictionary<string, int> playerScores;

    NetworkManager networkManager;
    Controls controls;

    [HideInInspector]
    public bool isActive;

    bool gameOver;

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

    void OnEnable()
    {
        controls = Controls.CreateWithDefaultBindings();
    }

    void OnDisable()
    {
        controls.Destroy();
    }

    IEnumerator Start()
    {
        networkManager = NetworkManager.singleton;
        minutes = GameCustomization.gameLength;
        seconds = 1;
        StartTimer();

        yield return new WaitForSeconds(5);
        Destroy(originalSpawnPoints);
        Instantiate(newSpawnPoints);
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

            if (minutes == 0 && seconds == 0)
            {
                gameOver = true;
                CheckScores();
                StartCoroutine(EndGame());
                break;
            }

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

    [Command]
    public void CmdAddScore(string player, int amount)
    {
        if (!playerScores.ContainsKey(player)) return;
        RpcAddScore(player, amount);
    }

    [ClientRpc]
    public void RpcAddScore(string player, int amount)
    {
        if (!playerScores.ContainsKey(player)) return;
        playerScores[player] += amount;
        CheckScores();
    }

    void CheckScores()
    {
        string winningPlayer = "";
        int lastAmt = 0;
        int killAmount = -1;
        foreach(string name in playerScores.Keys)
        {
            lastAmt = playerScores[name];
            if (playerScores[name] >= GameCustomization.pointsToWin)
            {
                winningPlayer = name;
                gameOver = true;
                break;
            }
            else
            {                
                if(lastAmt > killAmount)
                {
                    winningPlayer = name;
                    killAmount = lastAmt;
                }
            }            
        }

        if (gameOver)
        {
            CmdGameOver(winningPlayer);
        }
    }

    [Command]
    void CmdGameOver(string winningPlayer)
    {
        RpcGameOver(winningPlayer);
    }

    [ClientRpc]
    void RpcGameOver(string winningPlayer)
    {
        endGameText.text = winningPlayer + " won!" + '\n' + "Press the Jump button to continue";
        endGameText.gameObject.SetActive(true);
        StartCoroutine(EndGame());
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

    IEnumerator EndGame()
    {
        yield return new WaitUntil(ButtonPressed);
        LoadMainMenu();
    }

    bool ButtonPressed()
    {
        if (controls.Jump.WasPressed)
            return true;
        else
            return false;
    }

    public void LoadMainMenu()
    {
        MatchInfo matchInfo = networkManager.matchInfo;
        networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);
        networkManager.StopHost();
    }

    public void FlagCaptured(string player, int score)
    {
        CmdAddScore(player, score);
        Debug.LogError("FlagCaptured");
    }
}