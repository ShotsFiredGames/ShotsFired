using UnityEngine.UI;
using UnityEngine;

public class RoomListing : MonoBehaviour
{
    [SerializeField]
    Text roomNameText;

    public bool Updated { get; set; }
    public string RoomName { get; private set; }

	void Start ()
    {
		if(ServerLauncher.instance != null)
            GetComponent<Button>().onClick.AddListener(() => ServerLauncher.instance.JoinRoom(roomNameText.text));
	}

    void OnDestroy()
    {
        GetComponent<Button>().onClick.RemoveAllListeners();
    }

    public void SetRoomNameText(string text)
    {
        RoomName = text;
        roomNameText.text = RoomName;
    }
}
