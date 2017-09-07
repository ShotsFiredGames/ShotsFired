using UnityEngine;
using UnityEngine.UI;

public class TextToggle : MonoBehaviour
{
    public bool isActivate { get; set; }
    public string info { get; set; }

    Toggle toggle;

	void Start ()
    {
        toggle = GetComponent<Toggle>();
        info = GetComponentInChildren<Text>().text;
        isActivate = toggle.isOn;
    }
	
	void Update ()
    {
        isActivate = toggle.isOn;
	}
}
