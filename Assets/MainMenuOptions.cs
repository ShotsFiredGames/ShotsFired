using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using InControl;
using System.Text.RegularExpressions;
using UnityEngine.PostProcessing;

public class MainMenuOptions : MonoBehaviour {
    [Header("Main Menu Buttons")]
    public GameObject[] mainMenuBtns;
    [Space(10)]
    [Header("Options")]
    public GameObject[] mainOptionsSubBtns;
    [Space(10)]
    public GameObject[] displayMenuBtns;

    public GameObject[] controlOptionsBtns;
    public GameObject[] audioMenuButtons;
    //[SerializeField]
    //Text resolutionValue;
    [SerializeField]
    GameObject creditDisplay;
    public GameObject menuPanel, lobbyPanel;
    //[SerializeField]
    //string[] keyBindingTitles;
    [SerializeField]
    Toggle fullscreenToggle;
    //[SerializeField]
    //Button fireKeyBinding, moveLeftKeyBinding, moveRightKeyBinding, jumpKeyBinding, upKeyBinding, downKeyBinding, lookLeftKeyBinding, lookRightKeyBinding;
    [SerializeField]
    Dropdown resolutionDropdown, textureQualityDropdown, antialiasingDropdown, vSyncDropdown, ambientOcclusion, motionBlur;


    [SerializeField]
    Resolution[] resolutions;

    [SerializeField]
    AudioMixer mixer;
    [SerializeField]
    Slider masterVolumeSlider, musicVolumeSlider, sfxVolumeSlider, rotationSpeedSlider;
    [SerializeField]
    Toggle toggleMute;
    [SerializeField]
    PostProcessingProfile postProcs;

    private Controls controls;
    private GameSettings gameSettings = new GameSettings();
    private List<Slider> sliderList = new List<Slider>();
    private int resolutionIndex = 0;
    private bool muted = false, inUse = false, changeText = false;
    private int backspacingIndex = 0;
 

    private void OnEnabeled()
    {
        controls = new Controls();
        DontDestroyOnLoad(this.gameObject);
        postProcs.ambientOcclusion.enabled = false;
        postProcs.motionBlur.enabled = false;
        sliderList.Add(rotationSpeedSlider);

        if (PlayerPrefs.GetInt("NewGame") == 0)                                  //Check to see if this is the first time game ran on this computer
        {
            PlayerPrefs.SetInt("NewGame", 1);

            mixer.SetFloat("MasterVolume", masterVolumeSlider.value);
            mixer.SetFloat("MusicVolume", musicVolumeSlider.value);             //Sets volume to default values
        }
        else
        {
            if (PlayerPrefs.GetInt("Muted") == 0)
            {
                masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume");
                musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");

                mixer.SetFloat("MasterVolume", masterVolumeSlider.value);
                mixer.SetFloat("MusicVolume", musicVolumeSlider.value);             //Sets volume to previous settings
            }
            else
            {
                toggleMute.isOn = true;
                Mute();
            }
            gameSettings = new GameSettings();
            resolutionDropdown.onValueChanged.AddListener(delegate { OnResolutionChange(); });
            textureQualityDropdown.onValueChanged.AddListener(delegate { OnTextureQualityChange(); });
            antialiasingDropdown.onValueChanged.AddListener(delegate { OnAntialiasingChange(); });
            vSyncDropdown.onValueChanged.AddListener(delegate { OnVSyncChange(); });
           

        }
        resolutionIndex = 0;
        Screen.fullScreen = !Screen.fullScreen;
        if(!mainMenuBtns[0].activeSelf)
            ActivationOfButtons(mainMenuBtns, true);
        if(displayMenuBtns[0].activeSelf)
            ActivationOfButtons(displayMenuBtns, false);
        if(mainOptionsSubBtns[0].activeSelf)
            ActivationOfButtons(mainOptionsSubBtns, false);
        if(sliderList[0].IsActive())
            ActivationOfButtons(sliderList, false);
        if(audioMenuButtons[0].activeSelf)
            ActivationOfButtons(audioMenuButtons, false);
        if(controlOptionsBtns[0].activeSelf)
            ActivationOfButtons(controlOptionsBtns, false);
        if (!lobbyPanel.activeSelf)
            ActivationOfButtons(lobbyPanel, false);
      
    }
    private void Awake()
    {
        //foreach (Button _btn in GameObject.FindObjectsOfType<Button>())
        //{
        //    if (_btn.transform.Find("Text"))
        //        _btn.transform.Find("Text").GetComponentInChildren<Text>().text = "Stuff";
        //}
        resolutions = Screen.resolutions;
        foreach (Resolution resolution in resolutions)
        {
            resolutionDropdown.options.Add(new Dropdown.OptionData(resolution.ToString()));
        }
    }
    private void LateUpdate()
    {
        //Debug.Log(playerActionDictionary.Count);
        if (controls != null && changeText)
        {
            foreach (PlayerAction _pa in Controls.playerActionDictionary.Keys)
            {
                if (_pa.HasChanged)
                {
                    Debug.Log("has changed");
                    Controls.CreateNewBindings(_pa);
                    changeText = false;

                }
                if (_pa.Bindings.Count > 0)
                {
                    EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text = _pa.Bindings[0].Name.ToString();
                    changeText = false;
                    //Debug.Log(EventSystem.current.currentSelectedGameObject.name.ToString());
                }

                //Debug.Log(_pa.Bindings.Count + " " + Controls.playerActionDictionary.Keys.Count);
            }
        }
        //Debug.Log(controls.Jump.ActiveDevice.GetFirstPressedButton().Control.ToString());

    }
    public void KeyBinding(string _ActionToBind)
    {
        controls = new Controls();
        InputManager.AttachDevice(InputManager.ActiveDevice);
        switch (_ActionToBind)
        {
            case "Jump":
                foreach(PlayerAction _pa in Controls.playerActionDictionary.Keys)
                    if (_pa.IsListeningForBinding)
                        _pa.StopListeningForBinding();
                controls.Jump.ClearBindings();
                controls.Jump.ListenForBinding();
                changeText = true;
                           
                break;
            case "Fire":
                foreach (PlayerAction _pa in Controls.playerActionDictionary.Keys)
                    if (_pa.IsListeningForBinding)
                        _pa.StopListeningForBinding();
                controls.Fire.ClearBindings();
                controls.Fire.ListenForBinding();
                changeText = true;

                break;
            case "Aim":
                foreach (PlayerAction _pa in Controls.playerActionDictionary.Keys)
                    if (_pa.IsListeningForBinding)
                        _pa.StopListeningForBinding();
                controls.Aim.ClearBindings();
                controls.Aim.ListenForBinding();
                changeText = true;

                break;
            case "Sprint":
                foreach (PlayerAction _pa in Controls.playerActionDictionary.Keys)
                    if (_pa.IsListeningForBinding)
                        _pa.StopListeningForBinding();
                controls.Sprint.ClearBindings();
                controls.Sprint.ListenForBinding();
                changeText = true;

                break;
            case "Left":
                foreach (PlayerAction _pa in Controls.playerActionDictionary.Keys)
                    if (_pa.IsListeningForBinding)
                        _pa.StopListeningForBinding();
                controls.Left.ClearBindings();
                controls.Left.ListenForBinding();
                changeText = true;

                break;
            case "Right":
                foreach (PlayerAction _pa in Controls.playerActionDictionary.Keys)
                    if (_pa.IsListeningForBinding)
                        _pa.StopListeningForBinding();
                controls.Right.ClearBindings();
                controls.Right.ListenForBinding();
                changeText = true;

                break;
            case "Up":
                foreach (PlayerAction _pa in Controls.playerActionDictionary.Keys)
                    if (_pa.IsListeningForBinding)
                        _pa.StopListeningForBinding();
                controls.Up.ClearBindings();
                controls.Up.ListenForBinding();
                changeText = true;

                break;
            case "Down":
                foreach (PlayerAction _pa in Controls.playerActionDictionary.Keys)
                    if (_pa.IsListeningForBinding)
                        _pa.StopListeningForBinding();
                controls.Down.ClearBindings();
                controls.Down.ListenForBinding();
                changeText = true;

                break;
            case "Crouch":
                foreach (PlayerAction _pa in Controls.playerActionDictionary.Keys)
                    if (_pa.IsListeningForBinding)
                        _pa.StopListeningForBinding();
                controls.Crouch.ClearBindings();
                controls.Crouch.ListenForBinding();
                changeText = true;

                break;
            case "Look Left":
                foreach (PlayerAction _pa in Controls.playerActionDictionary.Keys)
                    if (_pa.IsListeningForBinding)
                        _pa.StopListeningForBinding();
                controls.LookLeft.ClearBindings();
                controls.LookLeft.ListenForBinding();
                changeText = true;

                break;
            case "Look Right":
                foreach (PlayerAction _pa in Controls.playerActionDictionary.Keys)
                    if (_pa.IsListeningForBinding)
                        _pa.StopListeningForBinding();
                controls.LookRight.ClearBindings();
                controls.LookRight.ListenForBinding();
                changeText = true;

                break;
            case "Look Up":
                foreach (PlayerAction _pa in Controls.playerActionDictionary.Keys)
                    if (_pa.IsListeningForBinding)
                        _pa.StopListeningForBinding();
                controls.LookUp.ClearBindings();
                controls.LookUp.ListenForBinding();
                changeText = true;

                break;
            case "Look Down":
                foreach (PlayerAction _pa in Controls.playerActionDictionary.Keys)
                    if (_pa.IsListeningForBinding)
                        _pa.StopListeningForBinding();
                controls.LookDown.ClearBindings();
                controls.LookDown.ListenForBinding();
                changeText = true;

                break;
            case "ScoreBoard":
                foreach (PlayerAction _pa in Controls.playerActionDictionary.Keys)
                    if (_pa.IsListeningForBinding)
                        _pa.StopListeningForBinding();
                controls.ScoreBoard.ClearBindings();
                controls.ScoreBoard.ListenForBinding();
                changeText = true;

                break;
        }
    }
    public void OnResolutionChange()
    {
        Screen.SetResolution(resolutions[resolutionDropdown.value].width, resolutions[resolutionDropdown.value].height, Screen.fullScreen);
        gameSettings.resolutionIndex = resolutionDropdown.value;
        //Debug.Log(Screen.currentResolution);
    }
    public void OnTextureQualityChange()
    {
        QualitySettings.masterTextureLimit = gameSettings.textureQuality = textureQualityDropdown.value;
        //Debug.Log(gameSettings.textureQuality);
    }
    public void OnAntialiasingChange()
    {
        QualitySettings.antiAliasing = (int)Mathf.Pow(2, antialiasingDropdown.value);
        gameSettings.antialiasing = antialiasingDropdown.value;
    }
    public void OnVSyncChange()
    {
        QualitySettings.vSyncCount = gameSettings.vSync = vSyncDropdown.value;
        //Debug.Log(vSyncDropdown.value);
    }
    public void OnClickCredits(int _backspaceIndex)
    {
        if (!inUse)
        {
            inUse = true;
            backspacingIndex += _backspaceIndex;
            ActivationOfButtons(creditDisplay, true);
        }
    }
    public void OnClickMainOptions(int _backspaceIndex)
    {
        backspacingIndex += _backspaceIndex;
        ActivationOfButtons(mainMenuBtns, false);
        ActivationOfButtons(displayMenuBtns, false);
        ActivationOfButtons(mainOptionsSubBtns, true);
        ActivationOfButtons(sliderList, false);
        ActivationOfButtons(creditDisplay, false);
        ActivationOfButtons(lobbyPanel, false);
    }
    public void OnClickDisplayBtn(int _backspaceIndex)
    {
        if (!inUse)
        {
            inUse = true;
            backspacingIndex += _backspaceIndex;
            ActivationOfButtons(displayMenuBtns, true);
        }
    }
    public void OnClickControl(int _backspaceIndex)
    {
        if (!inUse)
        {
            inUse = true;
            backspacingIndex += _backspaceIndex;
            ActivationOfButtons(controlOptionsBtns, true);

        }
    }
    public void OnClickAudioBtn(int _backspaceIndex)
    {
        if (!inUse)
        {
            inUse = true;
            backspacingIndex += _backspaceIndex;
            ActivationOfButtons(mainOptionsSubBtns, false);
            ActivationOfButtons(EventSystem.current.currentSelectedGameObject, true);
            ActivationOfButtons(audioMenuButtons, true);
        }
    }
    public void Lobby(int _backspaceIndex)
    {
        if (!inUse)
        {
            inUse = true;
            backspacingIndex = _backspaceIndex;
            menuPanel.SetActive(true);
            ActivationOfButtons(mainMenuBtns, false);
        }
    }
    public void MotionBlurChange()
    {
        postProcs.motionBlur.enabled = !postProcs.motionBlur.enabled;
        gameSettings.motionBlur = motionBlur.value;

    }
    public void AmbientOcclusionChange()
    {
        postProcs.ambientOcclusion.enabled = !postProcs.ambientOcclusion.enabled;
        gameSettings.ambientOcclusion = ambientOcclusion.value;
    }
    public void FullScreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
    public void MasterVolumeControl()
    {
        mixer.SetFloat("MasterVolume", masterVolumeSlider.value);
        PlayerPrefs.SetFloat("MasterVolume", masterVolumeSlider.value);
    }
    public void MusicVoluemControl()
    {
        mixer.SetFloat("MusicVolume", musicVolumeSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", musicVolumeSlider.value);
    }
    public void SoundEffectsControl()
    {
        mixer.SetFloat("EventSFXVolume", sfxVolumeSlider.value);
        mixer.SetFloat("GeneralSFXVolume", sfxVolumeSlider.value);
        mixer.SetFloat("GunSoundsVolume", sfxVolumeSlider.value);
    }
    public void MuteAll()
    {
        if (toggleMute.isOn)
            Mute();
        else
            UnMute();
    }
    private void Mute()
    {
        muted = true;
        toggleMute.isOn = true;
        masterVolumeSlider.value = -80;
        musicVolumeSlider.value = -80;          //Sets ui to old values
        sfxVolumeSlider.value = -80;

        masterVolumeSlider.interactable = false;
        musicVolumeSlider.interactable = false;
        sfxVolumeSlider.interactable = false;

        mixer.SetFloat("MasterVolume", -80);                                      //Mute all
        mixer.SetFloat("MusicVolume", -80);
        mixer.SetFloat("GunSoundsVolume", sfxVolumeSlider.value);
        mixer.SetFloat("GeneralSFXVolume", sfxVolumeSlider.value);
        mixer.SetFloat("EventSFXVolume", sfxVolumeSlider.value);
    }
    private void UnMute()
    {
        muted = false;
        toggleMute.isOn = false;

        PlayerPrefs.SetInt("Muted", 0);
        //print(PlayerPrefs.GetFloat("MasterVolume"));
        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");          //Sets ui to old values
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("EventSFXVolume", sfxVolumeSlider.value);

        masterVolumeSlider.interactable = true;
        musicVolumeSlider.interactable = true;
        sfxVolumeSlider.interactable = true;

        mixer.SetFloat("MasterVolume", masterVolumeSlider.value);
        mixer.SetFloat("MusicVolume", musicVolumeSlider.value);
        mixer.SetFloat("GunSoundsVolume", sfxVolumeSlider.value);
        mixer.SetFloat("GeneralSFXVolume", sfxVolumeSlider.value);
        mixer.SetFloat("EventSFXVolume", sfxVolumeSlider.value);//Sets old volume
    }
    public void LookSensitivityControl()
    {
        //PlayerPrefs.SetFloat("RotationSpeed", rotationSpeedSlider.value);
        gameSettings.rotationSensitivity = (int)rotationSpeedSlider.value;
    }
    public void LoadInfo()
    {
        
        if (File.Exists(Application.dataPath + "/SettingsSaveFile.csv"))
        {
            StreamReader sr;
            using (sr = new StreamReader(Application.dataPath + "/SettingsSaveFile.csv"))
            {
                string[] lines = System.IO.File.ReadAllLines(Application.dataPath + "/SettingsSaveFile.csv");
                foreach (string _s in lines)
                {
                    var line = sr.ReadLine();
                    if (line.StartsWith("vSync"))
                    {
                        string resultString = Regex.Match(line, @"\d+").Value;
                        int num = Int32.Parse(resultString);
                        gameSettings.SetGameSettings(num, vSyncDropdown, "vSync");
                        //Debug.Log("anti");
                    }
                    else if (line.StartsWith("Anti-aliasing"))
                    {
                        string resultString = Regex.Match(line, @"\d+").Value;
                        int num = Int32.Parse(resultString);
                        gameSettings.SetGameSettings(num, antialiasingDropdown, "antialiasing");
                    }
                    else if(line.StartsWith("Resolution Index"))
                    {
                        string resultString = Regex.Match(line, @"\d+").Value;
                        int num = Int32.Parse(resultString);
                        gameSettings.SetGameSettings(num, resolutionDropdown, "resolutionIndex");
                    }
                    else if (line.StartsWith("Texture Quality"))
                    {
                        string resultString = Regex.Match(line, @"\d+").Value;
                        int num = Int32.Parse(resultString);
                        gameSettings.SetGameSettings(num, textureQualityDropdown, "textureQuality");
                    }
                    else if (line.StartsWith("Rotation Sensitivity"))
                    {
                        string resultString = Regex.Match(line, @"\d+").Value;
                        int num = Int32.Parse(resultString);
                        gameSettings.SetGameSettings(num, resolutionDropdown, "resolutionIndex");
                    }
                    else if (line.StartsWith("Ambient Occlusion"))
                    {
                        string resultString = Regex.Match(line, @"\d+").Value;
                        int num = Int32.Parse(resultString);
                        gameSettings.SetGameSettings(num, ambientOcclusion, "ambientOcclusion");
                    }
                    else if (line.StartsWith("Motion Blur"))
                    {
                        string resultString = Regex.Match(line, @"\d+").Value;
                        int num = Int32.Parse(resultString);
                        gameSettings.SetGameSettings(num, motionBlur, "motionBlur");
                    }
                }
            }
            sr.Close();
        }
        else
            Debug.LogError("There is no file created with your stored information.");
    }
    public void Save()
    {
        //Debug.Log(Application.dataPath);
        if (File.Exists(Application.dataPath + "/SettingsSaveFile.csv"))
            WriteFileInfo(Application.dataPath + "/SettingsSaveFile");
        else
        {
            File.Create(Application.dataPath + "/SettingsSaveFile.csv");
            WriteFileInfo(Application.dataPath + "/SettingsSaveFile");
        }
    }
    private void WriteFileInfo(string _path)
    {
        System.IO.StreamWriter sw;
        using (sw = File.AppendText(_path))
        {
            File.WriteAllText(_path + ".csv", "vSync: " + gameSettings.vSync.ToString() +"\n" + "Rotation Sensitivity: " + rotationSpeedSlider.value.ToString() + "\n" + "Texture Quality: " + gameSettings.textureQuality.ToString() + "\n" + "Anti-aliasing: " + gameSettings.antialiasing.ToString() + "\n" + "Resolution Index: " + gameSettings.resolutionIndex.ToString() + "\n" + "Ambient Occlusion: " + gameSettings.ambientOcclusion.ToString() + "\n" + "Motion Blur: " + gameSettings.motionBlur.ToString());
        }
        sw.Close();
    }
    public void GoBack(int _backSub)
    {
        backspacingIndex += _backSub;
        if (backspacingIndex < 0)
            backspacingIndex = 0;
        switch (backspacingIndex)
        {
            case 0:
                inUse = false;
                ActivationOfButtons(mainMenuBtns, true);
                ActivationOfButtons(mainOptionsSubBtns, false);
                ActivationOfButtons(displayMenuBtns, false);
                ActivationOfButtons(controlOptionsBtns, false);
                ActivationOfButtons(lobbyPanel, false);
                break;
            case 1:
                inUse = false;
                ActivationOfButtons(mainMenuBtns, false);
                ActivationOfButtons(mainOptionsSubBtns, true);
                ActivationOfButtons(displayMenuBtns, false);
                ActivationOfButtons(audioMenuButtons, false);
                ActivationOfButtons(controlOptionsBtns, false);
                ActivationOfButtons(creditDisplay, false);
                ActivationOfButtons(lobbyPanel, false);
                break;
                
        }
    }
    public void ActivationOfButtons(GameObject _go, bool _activation)
    {
        _go.SetActive(_activation);
    }
    public void ActivationOfButtons(GameObject[] _btns, bool _activation)
    {
        foreach (GameObject _btn in _btns)
            _btn.SetActive(_activation);
        Debug.Log(_btns[0].gameObject.name.ToString());
    }
    public void ActivationOfButtons(List<Slider> _btns, bool _activation)
    {
        foreach (Slider _btn in _btns)
            _btn.gameObject.SetActive(_activation);
    }
}
