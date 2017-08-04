namespace InControl
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class MouseSettings : MonoBehaviour
    {
        Slider slider;
        // Use this for initialization
        void Start()
        {
            slider = GetComponent<Slider>();
            slider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        }

        void ValueChangeCheck()
        {
            MouseBindingSource.ScaleX = slider.value;
            MouseBindingSource.ScaleY = slider.value;
            MouseBindingSource.ScaleZ = slider.value;
        }
    }
}
