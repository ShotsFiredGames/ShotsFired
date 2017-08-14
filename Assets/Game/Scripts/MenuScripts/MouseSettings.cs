namespace InControl
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class MouseSettings : MonoBehaviour
    {
        public Slider mouseSlider;
        // Use this for initialization
        void Start()
        {
            mouseSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });

            if (PlayerPrefs.GetInt("MouseSettings") == 0)
            {
                PlayerPrefs.SetFloat("MouseLevels", mouseSlider.value);
                PlayerPrefs.SetInt("MouseSettings", 1);
            }
            else
            {
                mouseSlider.value = PlayerPrefs.GetFloat("MouseLevels");
                MouseBindingSource.ScaleX = mouseSlider.value;
                MouseBindingSource.ScaleY = mouseSlider.value;
                MouseBindingSource.ScaleZ = mouseSlider.value;
            }
            
        }

        void ValueChangeCheck()
        {
            print(mouseSlider.value + " print mouseSlider value");
            MouseBindingSource.ScaleX = mouseSlider.value;
            MouseBindingSource.ScaleY = mouseSlider.value;
            MouseBindingSource.ScaleZ = mouseSlider.value;
            PlayerPrefs.SetFloat("MouseLevels", mouseSlider.value);
        }
    }
}
