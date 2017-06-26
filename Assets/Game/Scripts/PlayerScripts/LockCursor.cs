using UnityEngine;

public class LockCursor : MonoBehaviour
{
    public bool lockCursor;

	void Start ()
    {
        if(lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
	}
}
