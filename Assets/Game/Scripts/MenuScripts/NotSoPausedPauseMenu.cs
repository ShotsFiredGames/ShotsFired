using UnityEngine;

public class NotSoPausedPauseMenu : MonoBehaviour {

    [SerializeField]
    GameObject pauseMenu;

    public static bool isOn = false;

    public bool lockCursor = true;
    private bool m_cursorIsLocked = true;

    void Start()
    {
        isOn = false;
        UpdateCursorLock(true);
    }
	
	void Update () {
        if (Input.GetKeyUp(KeyCode.Escape))
            TogglePauseMenu();
	}

    public void TogglePauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        isOn = pauseMenu.activeSelf;
        UpdateCursorLock(!isOn);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.Disconnect();
    }

    private void UpdateCursorLock(bool isLocked)
    {
        m_cursorIsLocked = isLocked;

        if (m_cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (!m_cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}