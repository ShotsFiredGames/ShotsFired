using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class ServerLauncher : Photon.PunBehaviour
{

    #region Public Variables
    public static ServerLauncher instance;

    public PhotonLogLevel LogLevel = PhotonLogLevel.Informational;
    [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so a new room will be created.")]
    public byte maxPlayersPerRoom = 6;
    [Tooltip("The UI Panel to let the user enter name, connect, and play")]
    public GameObject mainMenuPanel;
    [Tooltip("The UI Label to inform the user that the connection is in progress")]
    public GameObject progressLabel;
    public float Progress { get; private set; }
    public PhotonView PhotonView { get; private set; }
    public GameObject loadingScreen;
    #endregion

    #region Private Variables
    string _gameVersion = "1";                                              //This client's version number. Users are separated by version number
    bool isConnecting;

    [SerializeField]
    GameObject lobbyPanel;
    [SerializeField]
    Text _roomName;
    [SerializeField]
    GameObject _roomListingPrefab;
    [SerializeField]
    GameObject roomListingContent;
    List<RoomListing> roomListings = new List<RoomListing>();
    [SerializeField]
    GameObject currentRoomPanel;
    [SerializeField]
    GameObject roomStateButton;
    [SerializeField]
    GameObject customizeButton;
    [SerializeField]
    GameObject startMatchButton;
    [SerializeField]
    GameObject _playerListingPrefab;
    [SerializeField]
    GameObject playerListingContent;
    List<PlayerListing> playerListings = new List<PlayerListing>();
    [SerializeField]
    GameObject loadingBarPrefab;
    #endregion

    void Awake()
    {
        InitializeInstance();
        PhotonNetwork.logLevel = LogLevel;
        PhotonNetwork.autoJoinLobby = false;
        PhotonNetwork.automaticallySyncScene = false;
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    void InitializeInstance()
    {
        if (instance != null && instance == this)
        {
            InitializePhotonView();
            return;
        }

        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        InitializePhotonView();
    }

    void InitializePhotonView()
    {
        if (PhotonView != null && PhotonView == GetComponent<PhotonView>())
            return;

        if (!GetComponent<PhotonView>())
        {
            PhotonView = gameObject.AddComponent<PhotonView>();
            PhotonView.viewID = 998;
        }
    }

    void Start()
    {
        progressLabel.SetActive(false);
        ConnectToNetwork();
    }

    void ConnectToNetwork()
    {
        if (!PhotonNetwork.connected)
            PhotonNetwork.ConnectUsingSettings(_gameVersion);
    }

    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if(scene.buildIndex == 0)
        {
            InitializeInstance();
            ConnectToNetwork();
            playerListings.Clear();            
            if (!mainMenuPanel)
                mainMenuPanel = MainMenuObjectWrangler.Instance.mainMenuPanel;
            if(!progressLabel)
                progressLabel = MainMenuObjectWrangler.Instance.progressLabel;
            if (!loadingScreen)
                loadingScreen = DoNotDestroyOnLoad.Instance.loadingScreen;
            if(!lobbyPanel)
                lobbyPanel = MainMenuObjectWrangler.Instance.lobbyPanel;
            if(!_roomName)
                _roomName = MainMenuObjectWrangler.Instance._roomName;
            if(!roomListingContent)
                roomListingContent = MainMenuObjectWrangler.Instance.roomListingContent;
            if(!currentRoomPanel)
                currentRoomPanel = MainMenuObjectWrangler.Instance.currentRoomPanel;
            if(!roomStateButton)
                roomStateButton = MainMenuObjectWrangler.Instance.roomStateButton;
            if(!customizeButton)
                customizeButton = MainMenuObjectWrangler.Instance.customizeButton;
            if(!startMatchButton)
                startMatchButton = MainMenuObjectWrangler.Instance.startMatchButton;
            if(!playerListingContent)
                playerListingContent = MainMenuObjectWrangler.Instance.playerListingContent;
        }
    }

    #region OnClick Functions
    public void ConnectToRandom () {
        isConnecting = true;
        progressLabel.SetActive(true);
        mainMenuPanel.SetActive(false);
        if (PhotonNetwork.connected)
            PhotonNetwork.JoinRandomRoom();
        else
            PhotonNetwork.ConnectUsingSettings(_gameVersion);
	}

    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = maxPlayersPerRoom };
        if (PhotonNetwork.CreateRoom(_roomName.text, roomOptions, TypedLobby.Default))
        {
            Debug.Log("Created room successfully");
        }
        else
            Debug.Log("Create room failed to send");
    }

    public void JoinRoom(string roomName)
    {
        if (PhotonNetwork.JoinRoom(roomName))
            Debug.Log("Joined room: " + roomName);
        else
            Debug.Log("Join room failed");            
    }

    public void RoomState()
    {
        if (!PhotonNetwork.isMasterClient) return;

        PhotonNetwork.room.IsOpen = !PhotonNetwork.room.IsOpen;
        PhotonNetwork.room.IsVisible = PhotonNetwork.room.IsOpen;
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        lobbyPanel.SetActive(true);
        currentRoomPanel.SetActive(false);
    }

    public void StartMatch()
    {
        if (!PhotonNetwork.isMasterClient) return;
        PhotonNetwork.room.IsOpen = false;
        PhotonNetwork.room.IsVisible = false;
        PhotonView.RPC("RPC_StartLoading", PhotonTargets.All);
    }

    [PunRPC]
    void RPC_StartLoading()
    {
        if (PhotonNetwork.isMasterClient)
        {
            for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
            {
                string playerName = PhotonNetwork.playerList[i].NickName.Substring(0, PhotonNetwork.playerList[i].NickName.Length - 6);
                int viewID = PhotonNetwork.AllocateViewID();
                PhotonView.RPC("RPC_CreateLoadBar", PhotonTargets.All, viewID, playerName, PhotonNetwork.playerList[i]);     //
            }
        }

        StartCoroutine(StartLoading());
    }

    IEnumerator StartLoading()
    {
        AsyncOperation operation = PhotonNetwork.LoadLevelASync("Game");

        while(!operation.isDone)
        {
            Progress = Mathf.Clamp01(operation.progress / .9f);
            yield return null;
        }
    }

    [PunRPC]
    void RPC_CreateLoadBar(int viewID, string playerName, PhotonPlayer newOwner)   //
    {
        loadingScreen.SetActive(true);
        GameObject newLoadingBar = Instantiate(loadingBarPrefab, loadingScreen.transform);
        newLoadingBar.transform.Find("PlayerName").GetComponent<Text>().text = playerName;
        PhotonView newView = newLoadingBar.GetComponentInChildren<PhotonView>();
        newView.viewID = viewID;
        if (PhotonNetwork.isMasterClient)
            newView.TransferOwnership(newOwner);
    }

    #endregion

    void RoomReceived(RoomInfo room)
    {
        int index = roomListings.FindIndex(x => x.RoomName == room.Name);

        if(index == -1)
        {
            if(room.IsVisible && room.PlayerCount < room.MaxPlayers)
            {
                GameObject roomListingObj = Instantiate(_roomListingPrefab, roomListingContent.transform);                
                roomListings.Add(roomListingObj.GetComponent<RoomListing>());
                index = (roomListings.Count - 1);
            }
        }

        if(index != -1)
        {
            RoomListing rl = roomListings[index];
            rl.SetRoomNameText(room.Name);
            rl.Updated = true;
        }
    }

    void RemoveOldRooms()
    {
        List<RoomListing> removeRooms = new List<RoomListing>();
        foreach(RoomListing rl in roomListings)
        {
            if (!rl.Updated)
                removeRooms.Add(rl);
            else
                rl.Updated = false;
        }

        foreach(RoomListing rl in removeRooms)
        {
            if (rl != null)
            {
                GameObject rlObj = rl.gameObject;                
                Destroy(rlObj);
            }

            roomListings.Remove(rl);
        }
    }

    void PlayerLeftRoom(PhotonPlayer photonPlayer)
    {
        int index = playerListings.FindIndex(x => x.PhotonPlayer == photonPlayer);
        if(index != -1)
        {
            if (playerListings[index] != null)
            {
                Destroy(playerListings[index].gameObject);                
            }

            playerListings.RemoveAt(index);
        }
    }

    void PlayerJoinedRoom(PhotonPlayer photonPlayer)
    {
        if (photonPlayer == null) return;

        PlayerLeftRoom(photonPlayer);

        if (!PhotonNetwork.isMasterClient)
        {
            roomStateButton.SetActive(false);
            startMatchButton.SetActive(false);
            customizeButton.SetActive(false);
        }
        else if(PhotonNetwork.isMasterClient)
        {
            roomStateButton.SetActive(true);
            startMatchButton.SetActive(true);
            customizeButton.SetActive(true);
        }
        GameObject playerListingObj = Instantiate(_playerListingPrefab, playerListingContent.transform);
        PlayerListing pl = playerListingObj.GetComponent<PlayerListing>();
        pl.ApplyPhotonPlayer(photonPlayer);

        playerListings.Add(pl);
    }    

    #region Photon Methods

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster was called by PUN");
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnDisconnectedFromPhoton()
    {
        if(progressLabel != null)
            progressLabel.SetActive(false);
        if(mainMenuPanel != null)
            mainMenuPanel.SetActive(true);
        Debug.Log("OnDisconnectedFromPhoton was called by PUN");
    }

    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        Debug.Log("OnPhotonRandomJoinFailed was called by PUN. No random room found. Creating room...");
        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = maxPlayersPerRoom }, null);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom was called by PUN. Joined room");

        foreach(Transform child in playerListingContent.transform)
        {
            Destroy(child.gameObject);
        }

        currentRoomPanel.SetActive(true);
        lobbyPanel.SetActive(false);
        progressLabel.SetActive(false);

        PhotonPlayer[] photonPlayers = PhotonNetwork.playerList;
        for(int i = 0; i < photonPlayers.Length; i++)
        {
            PlayerJoinedRoom(photonPlayers[i]);
        }
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby was called by PUN. Joined lobby.");
    }

    public override void OnPhotonCreateRoomFailed(object[] codeAndMessage)
    {
        Debug.Log("Create room failed: " + codeAndMessage[1]);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Room created successfully");        
    }

    public override void OnReceivedRoomListUpdate()
    {
        RoomInfo[] rooms = PhotonNetwork.GetRoomList();
        foreach(RoomInfo room in rooms)
        {
            RoomReceived(room);
        }

        RemoveOldRooms();
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        Debug.Log("OnPhotonPlayerConnected was called by PUN");
        PlayerJoinedRoom(newPlayer);
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        Debug.Log("OnPhotonPlayerDisconnected was called by PUN.");
        PlayerLeftRoom(otherPlayer);               
    }

    //called when the master client changes. this will kick everyone from the game
    //public override void OnMasterClientSwitched(PhotonPlayer newMasterClient)
    //{
    //    LeaveRoom();
    //}
    #endregion
}