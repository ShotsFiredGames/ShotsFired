using UnityEngine;
using System.Collections;

public class RFX4_StartDelay : MonoBehaviour
{
    public GameObject ActivatedGameObject;
    public float Delay = 1;
    
	void OnEnable ()
    {
        ActivatedGameObject.SetActive(false);
        Invoke("ActivateGO", Delay);
	}
	
	void ActivateGO ()
    {
        ActivatedGameObject.SetActive(true);
	}

    void OnDisable()
    {
        CancelInvoke("ActivateGO");
    }
}
