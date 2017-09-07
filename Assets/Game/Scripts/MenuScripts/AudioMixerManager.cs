using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMixerManager : MonoBehaviour
{
    public AudioMixer mixer;

    public Slider[] volumeSliders;
    public string[] volumeNames;

    public Toggle muteToggle;
    bool muted;

    private void Start()
    {
        if (PlayerPrefs.GetInt("NewGame") == 0)                                  //Check to see if this is the first time game ran on this computer
        {
            PlayerPrefs.SetInt("NewGame", 1);
            SetMixerToValue();
        }
        else
        {
            if (PlayerPrefs.GetInt("Muted") == 0)
            {
                GetValueFromPrefs();
                SetMixerToValue();
            }
            else
            {
                muteToggle.isOn = true;
                Mute();
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerPrefs.DeleteAll();
            print("Prefs Reset");
        }
    }

    public void SetVolume(int index)
    {
        mixer.SetFloat(volumeNames[index], volumeSliders[index].value);

        if (!muted)
            PlayerPrefs.SetFloat(volumeNames[index], volumeSliders[index].value);
    }

    public void MuteAll()
    {
        if (PlayerPrefs.GetInt("Muted") == 0)
            Mute();
        else
            UnMute();
    }

    void Mute()
    {
        muted = true;
        muteToggle.isOn = true;

        PlayerPrefs.SetInt("Muted", 1);
        SetPrefsToValue();
        SetValueFromInt(-80);
        SetInterabilityOfSliders(false);
        SetMixerToValue();
    }

    void UnMute()
    {
        muted = false;
        muteToggle.isOn = false;

        PlayerPrefs.SetInt("Muted", 0);
        print(PlayerPrefs.GetFloat("MasterVolume"));
        GetValueFromPrefs();
        SetInterabilityOfSliders(true);
        SetMixerToValue();
    }

    void GetValueFromPrefs()
    {
        for (int i = 0; i < volumeSliders.Length; i++)
        {
            volumeSliders[i].value = PlayerPrefs.GetFloat(volumeNames[i]);
        }
    }

    void SetPrefsToValue()
    {
        for (int i = 0; i < volumeSliders.Length; i++)
        {
            PlayerPrefs.SetFloat(volumeNames[i], volumeSliders[i].value);
        }
    }

    void SetMixerToValue()
    {
        for (int mtv = 0; mtv < volumeSliders.Length; mtv++)
        {
            mixer.SetFloat(volumeNames[mtv], volumeSliders[mtv].value);
        }
    }

    void SetInterabilityOfSliders(bool isActive)
    {
        foreach (Slider slider in volumeSliders)
        {
            slider.interactable = isActive;
        }
    }

    void SetValueFromInt(int value)
    {
        foreach (Slider slider in volumeSliders)
        {
            slider.value = value;
        }
    }
}

