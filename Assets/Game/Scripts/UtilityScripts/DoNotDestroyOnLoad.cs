using UnityEngine;

public class DoNotDestroyOnLoad : MonoBehaviour {

	void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
