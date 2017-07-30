using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SliderDisplay : MonoBehaviour
{
    Text text;
    Slider display;

    string message;

    void Start()
    {
        text = GetComponent<Text>();
        display = transform.GetComponentInParent<Slider>();
        message = text.text;
    }

    void Update()
    {
        text.text = message + display.value;
    }
}
