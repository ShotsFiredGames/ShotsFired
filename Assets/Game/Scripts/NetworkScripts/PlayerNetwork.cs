using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerNetwork : MonoBehaviour
{
    public static PlayerNetwork instance;
    public string PlayerName { get; private set; }
    PhotonView photonView;
    int playersInGame = 0;

    void Awake()
    {
        instance = this;
        PlayerName = PhotonNetwork.playerName;
        photonView = GetComponent<PhotonView>();

        PhotonNetwork.sendRate = 60;
        PhotonNetwork.sendRateOnSerialize = 30;
		PhotonNetwork.UseRpcMonoBehaviourCache = true;


        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "Game")
        {
            if (PhotonNetwork.isMasterClient)
                MasterLoadedGame();
            else
                NonMasterLoadedGame();
        }
    }

    void MasterLoadedGame()
    {
        photonView.RPC("RPC_LoadedGameScene", PhotonTargets.MasterClient);
        //photonView.RPC("RPC_LoadGameOthers", PhotonTargets.Others);
    }

    void NonMasterLoadedGame()
    {
        photonView.RPC("RPC_LoadedGameScene", PhotonTargets.MasterClient);
    }

    [PunRPC]
    void RPC_LoadGameOthers()
    {
        PhotonNetwork.LoadLevel("Game");
    }

    [PunRPC]
    void RPC_LoadedGameScene()
    {
        playersInGame++;
        if(playersInGame == PhotonNetwork.playerList.Length)
        {
            Debug.Log("All players are in the game scene.");
            photonView.RPC("RPC_CreatePlayer", PhotonTargets.All);
            photonView.RPC("RPC_DestroyLobby", PhotonTargets.All);
            playersInGame = 0;
        }
    }

    [PunRPC]
    void RPC_DestroyLobby()
    {
        ServerLauncher.instance.loadingScreen.SetActive(false);
        foreach(Transform child in ServerLauncher.instance.loadingScreen.transform)
        {
            Destroy(child.gameObject);
        }
        Destroy(ServerLauncher.instance.gameObject.GetComponent<PhotonView>());
        //Destroy(ServerLauncher.instance.loadingScreen);
        //Destroy(ServerLauncher.instance.gameObject);
        StartCoroutine(CinematicCameraSystem.instance.StartCinematic());
    }

    [PunRPC]
    void RPC_CreatePlayer()
    {
        if(GameManager.instance != null)
        {
            if (GameManager.instance.playerPrefab != null)
            {
                int spawnIndex = Random.Range(0, GameManager.instance.originalSpawnPoints.Count);
                Transform spawnPoint = GameManager.instance.originalSpawnPoints[spawnIndex].transform;
                GameManager.instance.originalSpawnPoints.Remove(spawnPoint.gameObject);
                PhotonNetwork.Instantiate(GameManager.instance.playerPrefab.name, spawnPoint.position, spawnPoint.rotation, 0);
            }
        }
    }
}