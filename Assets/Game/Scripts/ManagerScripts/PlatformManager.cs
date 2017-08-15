using UnityEngine;
using UnityEngine.EventSystems;

public class PlatformManager : MonoBehaviour {

    public EventSystem es;
    public enum SystemType
    {
        XBOX,
        PS4,
        PC
    }

    public static SystemType systemType = SystemType.PC;

	// Use this for initialization
	void Start () {
        es = MainMenuObjectWrangler.Instance.eventSystem.GetComponent<EventSystem>();
#if UNITY_WSA_10_0
        Application.targetFrameRate = 30;
        systemType = SystemType.XBOX;
#elif UNITY_EDITOR
        Application.targetFrameRate = 60;
        systemType = SystemType.PC;
#elif UNITY_STANDALONE
        Application.targetFrameRate = 60;
        systemType = SystemType.PC;
#endif
    }
}
