using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour {

    public EventSystem es;

	// Use this for initialization
	void Start () {
#if UNITY_WSA_10_0
#elif UNITY_EDITOR
        Debug.LogError("editor");
#endif
    }
}
