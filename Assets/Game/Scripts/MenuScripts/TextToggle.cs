using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextToggle : MonoBehaviour
{
    public bool isActivate { get; set; }
    public string info { get; set; }

    Toggle toggle;

	// Use this for initialization
	void Start ()
    {
        toggle = GetComponent<Toggle>();
        info = GetComponentInChildren<Text>().text;
	}
	
	// Update is called once per frame
	void Update ()
    {
        isActivate = toggle.isOn;
	}
}
