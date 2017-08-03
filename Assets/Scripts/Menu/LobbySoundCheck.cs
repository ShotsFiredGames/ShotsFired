using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LobbySoundCheck : MonoBehaviour
{
    public GameObject menuSound;
    public GameObject lobbySound;

	// Update is called once per frame
	void Update ()
    {
        if (!SceneManager.GetActiveScene().name.Equals("Lobby"))
            lobbySound.SetActive(false);
        else
            lobbySound.SetActive(!menuSound.activeSelf); 
    }
}
