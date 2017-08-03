using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class DisplaySettingsManager : MonoBehaviour
{
    public Toggle fullscreenToggle;
    public Dropdown resolutionDropdown;
    public Dropdown textureQualityDropdown;
    public Dropdown antialiasingDropdown;
    public Dropdown vSyncDropdown;
    public Button applyButton;

    public Resolution[] resolutions;
    public GameSettings gameSettings;

    bool savedSettings;

    void OnEnable()
    {
        gameSettings = new GameSettings();
        fullscreenToggle.onValueChanged.AddListener(delegate { OnFullscreenToggle(); });
        resolutionDropdown.onValueChanged.AddListener(delegate { OnResolutionChange(); });
        textureQualityDropdown.onValueChanged.AddListener(delegate { OnTextureQualityChange(); });
        antialiasingDropdown.onValueChanged.AddListener(delegate { OnAntialiasingChange(); });
        vSyncDropdown.onValueChanged.AddListener(delegate { OnVSyncChange(); });
        applyButton.onClick.AddListener(delegate { OnApplyButtonClicked(); });
        resolutions = Screen.resolutions;
        foreach (Resolution resolution in resolutions)
        {
            resolutionDropdown.options.Add(new Dropdown.OptionData(resolution.ToString()));
        }

        if (savedSettings)
            LoadSettings();
    }

    public void OnFullscreenToggle()
    {
        Screen.fullScreen = gameSettings.fullscreen = fullscreenToggle.isOn;
    }

    public void OnResolutionChange()
    {
        Screen.SetResolution(resolutions[resolutionDropdown.value].width, resolutions[resolutionDropdown.value].height, Screen.fullScreen);
        gameSettings.resolutionIndex = resolutionDropdown.value;
    }

    public void OnTextureQualityChange()
    {
        QualitySettings.masterTextureLimit = gameSettings.textureQuality = textureQualityDropdown.value;
    }

    public void OnAntialiasingChange()
    {
        QualitySettings.antiAliasing = (int)Mathf.Pow(2, antialiasingDropdown.value);
        gameSettings.antialiasing = antialiasingDropdown.value;
    }

    public void OnVSyncChange()
    {
        QualitySettings.vSyncCount = gameSettings.vSync = vSyncDropdown.value;
    }

    public void OnApplyButtonClicked()
    {
        SaveSettings();
    }

    public void SaveSettings()
    {
        string jsonData = JsonUtility.ToJson(gameSettings, true);
        File.WriteAllText(Application.persistentDataPath + "/gamesettings.json", jsonData);
        savedSettings = true;
    }

    public void LoadSettings()
    {
        try
        {
            gameSettings = JsonUtility.FromJson<GameSettings>(File.ReadAllText(Application.persistentDataPath + "/gamesettings.json"));
            fullscreenToggle.isOn = gameSettings.fullscreen;
            resolutionDropdown.value = gameSettings.resolutionIndex;
            textureQualityDropdown.value = gameSettings.textureQuality;
            antialiasingDropdown.value = gameSettings.antialiasing;
            vSyncDropdown.value = gameSettings.vSync;

            Screen.fullScreen = gameSettings.fullscreen;
            resolutionDropdown.RefreshShownValue();
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
}
