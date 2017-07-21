using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class GameSettings : MonoBehaviour
{
    public bool fullscreen;
    public int textureQuality;
    public int antialiasing;
    public int vSync;
    public int resolutionIndex;
    public int rotationSensitivity;
    //public bool ambientToggleSetting;


    public void SetGameSettings(int _value, Dropdown _drop, string _valueToChange)
    {
        _drop.value = _value;
        switch (_valueToChange)
        {
            case "textureQuality":
                textureQuality = _value;
                break;
            case "antialiasing":
                antialiasing = _value;
                break;
            case "vSync":
                vSync = _value;
                break;
            case "resolutionIndex":
                resolutionIndex = _value;
                break;
        }
    }
    public void SetGameSettings(int _value, Slider _slider)
    {

    }
}

