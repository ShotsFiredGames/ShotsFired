using UnityEngine;
using UnityEngine.UI;

public class MainMenuObjectWrangler : MonoBehaviour {

    public static MainMenuObjectWrangler Instance { get; private set; }

    public GameObject mainMenuPanel;
    public GameObject progressLabel;
    public GameObject lobbyPanel;
    public Text _roomName;
    public GameObject roomListingContent;
    public GameObject currentRoomPanel;
    public GameObject roomStateButton;
    public GameObject customizeButton;
    public GameObject startMatchButton;
    public GameObject playerListingContent;
    public GameObject eventSystem;

    void Awake()
    {
        Instance = this;
    }
}