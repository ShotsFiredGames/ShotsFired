using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class GameSettings : MonoBehaviour
{
    public int fullscreen;
    public int textureQuality;
    public int antialiasing;
    public int vSync;
    public int resolutionIndex;
    public int rotationSensitivity;
    public int ambientOcclusion;
    public int motionBlur;
    //public bool ambientToggleSetting;

    private void OnEnable()
    {
        Controls.CreateWithDefaultBindings();
    }
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
            case "ambientOcclusion":
                ambientOcclusion = _value;
                break;
            case "motionBlur":
                motionBlur = _value;
                break;
        }
    }
}

