using UnityEngine;
using UnityEngine.EventSystems;

public class SetSelected : MonoBehaviour {


    EventSystem eventSystem;

    void OnEnable()
    {
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        eventSystem.SetSelectedGameObject(gameObject);
    }
}
