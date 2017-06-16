using UnityEngine;

public class MenuCursor : MonoBehaviour {

	void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
