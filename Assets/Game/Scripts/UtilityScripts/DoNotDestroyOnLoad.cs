using UnityEngine;

public class DoNotDestroyOnLoad : MonoBehaviour {

    static DoNotDestroyOnLoad instance;

    public GameObject playerNetwork;
    static GameObject playerNetworkInstance;

	void Awake()
    {
        DontDestroyOnLoad(this);

        if (instance == null)
            instance = this;
        else
            DestroyObject(gameObject);

        if (playerNetworkInstance == null)
        {
            playerNetworkInstance = Instantiate(playerNetwork, transform);
            playerNetworkInstance.GetComponent<PhotonView>().viewID = 999;
        }
    }
}
