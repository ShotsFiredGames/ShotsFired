using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : Photon.PunBehaviour
{
    public static GameManager instance;
    public GameObject playerPrefab;
    public List<GameObject> originalSpawnPoints;
    public List<GameObject> newSpawnPoints;
    public Text timerText;
    public Text endGameText;
    public Text countDownTimer;
    public GameObject scorePanel;
    public GameObject scoreboardTextObj;
    [HideInInspector]
    public List<Text> scoreboardTextObjList;

    public PhotonView PhotonView { get; private set; }

    public Dictionary<string, short> playerScores;

    Controls controls;
    Animator countdownAnim;

    [HideInInspector]
    public bool isActive;

    bool gameOver;

    byte minutes;
    byte seconds;
    byte countdownTime;
    byte playersInGame;

    #region Photon Methods
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    void LoadArena()
    {
        if (!PhotonNetwork.isMasterClient)
            Debug.LogError("PhotonNetwork: Trying to Load a level but we are not the master client");

        Debug.Log("PhotonNetwork : Loading Level");
        PhotonNetwork.LoadLevel("Game");
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        Debug.Log("OnPhotonPlayerConnected() " + newPlayer.NickName);

        if (PhotonNetwork.isMasterClient)
            Debug.Log("OnPhotonPlayerConnected isMasterClient " + PhotonNetwork.isMasterClient);
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer other)
    {
        Debug.Log("OnPhotonPlayerDisconnected() " + other.NickName);

        if (PhotonNetwork.isMasterClient)
            Debug.Log("OnPhotonPlayerDisonnected isMasterClient " + PhotonNetwork.isMasterClient);
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            if (PhotonNetwork.isMasterClient)
            {
                stream.SendNext(minutes);
                stream.SendNext(seconds);
                stream.SendNext(countdownTime);
            }
        }
        else
        {
            minutes = (byte)stream.ReceiveNext();
            seconds = (byte)stream.ReceiveNext();
            countdownTime = (byte)stream.ReceiveNext();
        }
    }
    #endregion

    void Awake()
    {
        if (instance != null)
            Debug.LogError("More than one GameManager in scene.");
        else
            instance = this;

        PhotonView = GetComponent<PhotonView>();
        playerScores = new Dictionary<string, short>();
        countdownAnim = countDownTimer.GetComponent<Animator>();
    }

    void OnEnable()
    {
        controls = Controls.CreateWithDefaultBindings();
        CinematicCameraSystem.OnCinematicFinished += CinematicIsDone;
    }

    void OnDisable()
    {
        controls.Destroy();
        CinematicCameraSystem.OnCinematicFinished -= CinematicIsDone;
    }

    void Start()
    {
        minutes = GameCustomization.gameLength;
    }

    [PunRPC]
    void RPC_CinematicIsDone()
    {
        playersInGame++;
        if(playersInGame == PhotonNetwork.playerList.Length)
            StartGame();
    }

    void CinematicIsDone()
    {
        photonView.RPC("RPC_CinematicIsDone", PhotonTargets.All);
    }

    void StartGame()
    {
        countDownTimer.enabled = true;
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        for(byte i = 10; i > 0; i--)
        {
            if(PhotonNetwork.isMasterClient)
                countdownTime = i;

            countdownAnim.SetTrigger("Countdown");
            countDownTimer.text = countdownTime.ToString();
            yield return new WaitForSeconds(1);
        }
        countDownTimer.text = "Fight!";
        yield return new WaitForSeconds(1);
        countDownTimer.enabled = false;

        timerText.enabled = true;
        if (PhotonNetwork.isMasterClient)
        {
            StartCoroutine(Timer());
            byte eventcode = 0;
            RaiseEventOptions eventOptions = new RaiseEventOptions();
            eventOptions.Receivers = ReceiverGroup.All;
            PhotonNetwork.RaiseEvent(eventcode, true, true, eventOptions);
        }
    }

    IEnumerator Timer()
    {
        for(byte i = seconds; i >= 0; i--)
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

    [PunRPC]
    public void RPC_AddScore(string player, short amount)
    {
        if (player == null) return;
        if (!playerScores.ContainsKey(player)) return;
        playerScores[player] += amount;

        if (playerScores[player] < 0) //check to prevent scores from being negative
            playerScores[player] = 0;

        CheckScores();
    }

    void CheckScores()
    {
        string winningPlayer = GetWinningPlayer();

        if (playerScores[winningPlayer] >= GameCustomization.pointsToWin)
            gameOver = true;

        if (gameOver)
            GameOver(winningPlayer);
    }

    void GameOver(string winningPlayer)
    {
        endGameText.text = winningPlayer + " won!" + '\n' + "Press the Jump button to continue";
        endGameText.gameObject.SetActive(true);
        StartCoroutine(EndGame());

        if (PhotonNetwork.isMasterClient)
        {
            int arrayIndex = AnnouncerManager.instance.GetRandomIndex(AnnouncerManager.instance.generalClips.endMatch.Length);
            AnnouncerManager.instance.PhotonView.RPC("RPC_PlayEndMatchClip", PhotonTargets.All, arrayIndex);
        }
    }

    void Update()
    {
        UpdateTimerText();
    }

    void UpdateTimerText()
    {
        timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
    }

    void UpdateScoreText()
    {
        byte count = 0;
        foreach(string name in playerScores.Keys)
        {
            scoreboardTextObjList[count].text = name + ": " + playerScores[name];
            count++;
        }
    }

    IEnumerator EndGame()
    {
        yield return new WaitUntil(ButtonPressed);
        LeaveRoom();
    }

    bool ButtonPressed()
    {
        if (controls.Jump.WasPressed)
            return true;
        else
            return false;
    }

    //==========Event Methods==========
	public void FlagCaptured(string player, short score)
    {
        photonView.RPC("RPC_AddScore", PhotonTargets.All, player, score);
    }

    public string GetWinningPlayer()
    {
        short highScore = short.MinValue;
        string playerName = "";
        foreach (string name in playerScores.Keys)
        {
            if (playerScores[name] > highScore)
            {
                highScore = playerScores[name];
                playerName = name;
            }
        }

        return playerName;
    }

    public Transform GetSpawnPoint()
    {
        return newSpawnPoints[Random.Range(0, newSpawnPoints.Count)].transform;
    }
}