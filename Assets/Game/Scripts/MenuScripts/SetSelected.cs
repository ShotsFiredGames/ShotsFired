using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SetSelected : MonoBehaviour {

	void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }
}
