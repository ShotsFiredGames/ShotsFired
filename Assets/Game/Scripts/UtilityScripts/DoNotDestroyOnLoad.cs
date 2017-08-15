using UnityEngine;

public class DoNotDestroyOnLoad : MonoBehaviour {

    public static DoNotDestroyOnLoad Instance { get; private set; }

    public GameObject playerNetwork;
    static GameObject playerNetworkInstance;
    public GameObject menuManagerPrefab;
    static GameObject menuManagerInstance;
    public GameObject loadingScreen;

	void Awake()
    {
        DontDestroyOnLoad(this);

        if (Instance == null)
            Instance = this;
        else
            DestroyObject(gameObject);

        if (playerNetworkInstance == null)
        {
            playerNetworkInstance = Instantiate(playerNetwork, transform);
            playerNetworkInstance.GetComponent<PhotonView>().viewID = 999;
        }

        if(menuManagerInstance == null)
        {
            menuManagerInstance = Instantiate(menuManagerPrefab, transform);
        }
    }
}
