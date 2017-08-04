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
        display.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        message = text.text;
        text.text = message + display.value.ToString("F2");
    }

    void ValueChangeCheck()
    {
        text.text = message + display.value.ToString("F2");
    }
}
