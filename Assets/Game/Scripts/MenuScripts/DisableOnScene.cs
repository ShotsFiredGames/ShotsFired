using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DisableOnScene : MonoBehaviour {

    public string sceneName;
    public GameObject objectToActivate;

	// Update is called once per frame
	void FixedUpdate ()
    {
        if (SceneManager.GetActiveScene().name.Equals(sceneName))
        {
            if (objectToActivate.activeSelf)
                objectToActivate.SetActive(false);
        }
        else
        {
            if (!objectToActivate.activeSelf)
                objectToActivate.SetActive(true);
        }
	}
}
