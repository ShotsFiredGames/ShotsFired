using UnityEngine;
using UnityEngine.UI;

public class StartMenuScript : MonoBehaviour
{
    public Button quickPlay;
    public Button startMatch;
    public Button roomState;
    public Button leaveRoom;
    public Button createRoom;

    void Start()
    {
        quickPlay.onClick.AddListener(ServerLauncher.instance.ConnectToRandom);
        startMatch.onClick.AddListener(ServerLauncher.instance.StartMatch);
        roomState.onClick.AddListener(ServerLauncher.instance.RoomState);
        leaveRoom.onClick.AddListener(ServerLauncher.instance.LeaveRoom);
        createRoom.onClick.AddListener(ServerLauncher.instance.CreateRoom);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}