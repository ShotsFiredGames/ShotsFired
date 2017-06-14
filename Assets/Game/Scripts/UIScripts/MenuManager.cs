using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour {

    public EventSystem es;

	// Use this for initialization
	void Start () {
#if UNITY_WSA_10_0
        Application.targetFrameRate = 30;
#elif UNITY_EDITOR
        Application.targetFrameRate = 60;
#elif UNITY_STANDALONE
        Application.targetFrameRate = 60;
#endif
    }
}
