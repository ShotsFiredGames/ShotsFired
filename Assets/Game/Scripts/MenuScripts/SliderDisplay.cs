using UnityEngine;
using UnityEngine.UI;

public class SliderDisplay : MonoBehaviour
{
    public int decimalPoints;
    Text text;
    Slider display;

    string message;
    string formatString;

    void Start()
    {
        text = GetComponent<Text>();
        display = transform.GetComponentInParent<Slider>();
        display.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        message = text.text;
        formatString = "F" + decimalPoints.ToString();
        text.text = message + display.value.ToString(formatString);
    }

    void ValueChangeCheck()
    {
        text.text = message + display.value.ToString(formatString);
    }
}
