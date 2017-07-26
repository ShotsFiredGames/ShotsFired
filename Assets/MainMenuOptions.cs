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

    public GameObject[] mainMenuBtns, mainOptionsSubBtns, displayMenuBtns, controlOptionsBtns;
    //[SerializeField]
    //Text resolutionValue;
    [SerializeField]
    GameObject creditDisplay;
    public GameObject menuPanel;
    [SerializeField]
    string[] keyBindingTitles;
    [SerializeField]
    Toggle fullscreenToggle;
    //[SerializeField]
    //Button fireKeyBinding, moveLeftKeyBinding, moveRightKeyBinding, jumpKeyBinding, upKeyBinding, downKeyBinding, lookLeftKeyBinding, lookRightKeyBinding;
    [SerializeField]
    Dropdown resolutionDropdown, textureQualityDropdown, antialiasingDropdown, vSyncDropdown;


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
    private GameObject[] audioSliders;
    private GameSettings gameSettings = new GameSettings();
    private List<Slider> sliderList = new List<Slider>();
    private int resolutionIndex = 0;
    private bool muted = false, inUse = false;
    private int backspacingIndex = 0;
 

    private void OnEnabeled()
    {
        controls = new Controls();
        DontDestroyOnLoad(this.gameObject);
        //byte i = 0;
        //foreach(PlayerAction _pa in controls.Actions)
        //{
        //    i++;
        //    playerActionDictionary.Add(_pa, i);
        //    //Debug.Log(playerActionDictionary.Keys + " " + playerActionDictionary.ContainsValue(i).ToString());
        //}
        postProcs.ambientOcclusion.enabled = false;
        postProcs.motionBlur.enabled = false;
        sliderList.Add(masterVolumeSlider);
        sliderList.Add(musicVolumeSlider);
        sliderList.Add(sfxVolumeSlider);
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
            //fullscreenToggle.onValueChanged.AddListener(delegate { OnFullscreenToggle(); });
            resolutionDropdown.onValueChanged.AddListener(delegate { OnResolutionChange(); });
            textureQualityDropdown.onValueChanged.AddListener(delegate { OnTextureQualityChange(); });
            antialiasingDropdown.onValueChanged.AddListener(delegate { OnAntialiasingChange(); });
            vSyncDropdown.onValueChanged.AddListener(delegate { OnVSyncChange(); });
            //applyButton.onClick.AddListener(delegate { OnApplyButtonClicked(); });
            resolutions = Screen.resolutions;
            foreach (Resolution resolution in resolutions)
            {
                resolutionDropdown.options.Add(new Dropdown.OptionData(resolution.ToString()));
            }

        }
        resolutionIndex = 0;
        Screen.fullScreen = !Screen.fullScreen;
        audioSliders = GameObject.FindGameObjectsWithTag("Volume");
        if(!mainMenuBtns[0].activeSelf)
            ActivationOfButtons(mainMenuBtns, true);
        if(displayMenuBtns[0].activeSelf)
            ActivationOfButtons(displayMenuBtns, false);
        if(mainOptionsSubBtns[0].activeSelf)
            ActivationOfButtons(mainOptionsSubBtns, false);
        if(sliderList[0].IsActive())
            ActivationOfButtons(sliderList, false);
        if(audioSliders[0].activeSelf)
            ActivationOfButtons(audioSliders, false);
        if(controlOptionsBtns[0].activeSelf)
            ActivationOfButtons(controlOptionsBtns, false);

    }
    //private Dictionary<string, Key> playerActionDictionary = new Dictionary<string, Key>();
    //private void SaveKeyBindings(string _key, Key _value)
    //{
    //    if(File.Exists(Application.dataPath + "/SettingsSaveFile"))
    //    {
    //        foreach (KeyValuePair<string, Key> kvp in playerActionDictionary)
    //        {
    //            File.AppendAllText(Application.dataPath + "/SettingsSaveFile.csv", kvp.Key.ToString() + " " + kvp.Value.ToString() + "\n");
    //        }
    //    }
    //}
    private void FixedUpdate()
    {
        //Debug.Log(playerActionDictionary.Count);
        if (controls != null)
        {
            foreach (PlayerAction _pa in Controls.playerActionDictionary.Keys)
            {
                if (_pa.HasChanged)
                {
                    Debug.Log("has changed");
                    Controls.CreateNewBindings(_pa);
                }

                Debug.Log(_pa.Bindings.Count + " " + Controls.playerActionDictionary.Keys.Count);
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
                           
                break;
            case "Fire":
                Debug.Log(controls.Fire.Bindings.Count);
                controls.Fire.ClearBindings();
                controls.Fire.ListenForBinding();
                if (!controls.Fire.IsListeningForBinding)
                    Debug.Log(controls.Fire.Bindings.Count);
                controls.Save();
                PlayerPrefs.SetString("SaveInfoFire", controls.Save().ToString());
                Debug.Log(controls.Save().ToString());

                break;
            case "Aim":
                Debug.Log(controls.Aim.Bindings.Count);
                controls.Aim.ClearBindings();
                controls.Aim.ListenForBinding();
                if (!controls.Aim.IsListeningForBinding)
                    Debug.Log(controls.Aim.Bindings.Count);
                controls.Save();
                PlayerPrefs.SetString("SaveInfoAim", controls.Save().ToString());
                Debug.Log(controls.Save().ToString());

                break;
            case "Sprint":
                Debug.Log(controls.Sprint.Bindings.Count);
                controls.Sprint.ClearBindings();
                controls.Sprint.ListenForBinding();
                if (!controls.Sprint.IsListeningForBinding)
                    Debug.Log(controls.Sprint.Bindings.Count);
                controls.Save();
                PlayerPrefs.SetString("SaveInfoSprint", controls.Save().ToString());
                Debug.Log(controls.Save().ToString());

                break;
            case "Left":
                controls.Left.ClearBindings();
                controls.Left.ListenForBinding();
                if (!controls.Left.IsListeningForBinding)
                    Debug.Log(controls.Left.Bindings.Count);
                controls.Save();
                PlayerPrefs.SetString("SaveInfoLeft", controls.Save().ToString());
                Debug.Log(controls.Save().ToString());

                break;
            case "Right":
                controls.Right.ClearBindings();
                controls.Right.ListenForBinding();
                if (!controls.Right.IsListeningForBinding)
                    Debug.Log(controls.Right.Bindings.Count);
                controls.Save();
                PlayerPrefs.SetString("SaveInfoRight", controls.Save().ToString());
                Debug.Log(controls.Save().ToString());

                break;
            case "Up":
                controls.Up.ClearBindings();
                controls.Up.ListenForBinding();
                if (!controls.Up.IsListeningForBinding)
                    Debug.Log(controls.Up.Bindings.Count);
                controls.Save();
                PlayerPrefs.SetString("SaveInfoUp", controls.Save().ToString());
                Debug.Log(controls.Save().ToString());

                break;
            case "Down":
                controls.Down.ClearBindings();
                controls.Down.ListenForBinding();
                if (!controls.Down.IsListeningForBinding)
                    Debug.Log(controls.Down.Bindings.Count);
                controls.Save();
                PlayerPrefs.SetString("SaveInfoDown", controls.Save().ToString());
                Debug.Log(controls.Save().ToString());

                break;
            case "Crouch":
                controls.Crouch.ClearBindings();
                controls.Crouch.ListenForBinding();
                if (!controls.Crouch.IsListeningForBinding)
                    Debug.Log(controls.Crouch.Bindings.Count);
                controls.Save();
                PlayerPrefs.SetString("SaveInfoCrouch", controls.Save().ToString());
                Debug.Log(controls.Save().ToString());

                break;
            case "Look Left":
                controls.LookLeft.ClearBindings();
                controls.LookLeft.ListenForBinding();
                if (!controls.LookLeft.IsListeningForBinding)
                    Debug.Log(controls.LookLeft.Bindings.Count);
                controls.Save();
                PlayerPrefs.SetString("SaveInfoLookLeft", controls.Save().ToString());
                Debug.Log(controls.Save().ToString());

                break;
            case "Look Right":
                controls.LookRight.ClearBindings();
                controls.LookRight.ListenForBinding();
                if (!controls.LookRight.IsListeningForBinding)
                    Debug.Log(controls.LookRight.Bindings.Count);
                controls.Save();
                PlayerPrefs.SetString("SaveInfoLookRight", controls.Save().ToString());
                Debug.Log(controls.Save().ToString());

                break;
            case "Look Up":
                controls.LookUp.ClearBindings();
                controls.LookUp.ListenForBinding();
                if (!controls.LookUp.IsListeningForBinding)
                    Debug.Log(controls.LookUp.Bindings.Count);
                controls.Save();
                PlayerPrefs.SetString("SaveInfoLookUp", controls.Save().ToString());
                Debug.Log(controls.Save().ToString());

                break;
            case "Look Down":
                controls.LookDown.ClearBindings();
                controls.LookDown.ListenForBinding();
                if (!controls.LookDown.IsListeningForBinding)
                    Debug.Log(controls.LookDown.Bindings.Count);
                controls.Save();
                PlayerPrefs.SetString("SaveInfoLookDown", controls.Save().ToString());
                Debug.Log(controls.Save().ToString());

                break;
            case "ScoreBoard":
                controls.ScoreBoard.ClearBindings();
                controls.ScoreBoard.ListenForBinding();
                if (!controls.ScoreBoard.IsListeningForBinding)
                    Debug.Log(controls.ScoreBoard.Bindings.Count);
                controls.Save();
                PlayerPrefs.SetString("SaveInfoScoreBoard", controls.Save().ToString());
                Debug.Log(controls.Save().ToString());

                break;
        }
    }
    //public void OnClickKeyBindMoveLeft()
    //{
    //    if (!moveLeftKeyBinding.IsActive())
    //        ActivationOfButtons(moveLeftKeyBinding.gameObject, true);
    //}
    //public void MoveLeftKeyBind()
    //{
    //    Key kc = (Key)System.Enum.Parse(typeof(KeyCode), moveLeftKeyBinding.text);
    //    controls.Left.AddDefaultBinding(kc);
    //    playerActionDictionary.Add("Move Left", kc);
    //    SaveKeyBindings("Move Left", kc);
    //    Debug.Log(controls.Left);
    //}
    //public void OnClickKeyBindMoveRight()
    //{
    //    if (!moveRightKeyBinding.IsActive())
    //        ActivationOfButtons(moveRightKeyBinding.gameObject, true);
    //}
    //public void MoveRightKeyBind()
    //{
    //    Key kc = (Key)System.Enum.Parse(typeof(KeyCode), moveRightKeyBinding.text);
    //    controls.Right.AddDefaultBinding(kc);
    //    playerActionDictionary.Add("Move Right", kc);
    //    SaveKeyBindings("Move Right", kc);
    //    Debug.Log(controls.Right);
    //}
    //public void OnClickKeyBindJump()
    //{
    //    if (!jumpKeyBinding.IsActive())
    //        ActivationOfButtons(jumpKeyBinding.gameObject, true);
    //}
    //public void JumpKeyBind()
    //{
    //    Key kc = (Key)System.Enum.Parse(typeof(KeyCode), jumpKeyBinding.text);
    //    controls.Jump.ClearBindings();
    //    controls.Jump.AddDefaultBinding(kc);
    //    playerActionDictionary.Add("Jump", kc); ;
    //    SaveKeyBindings("Jump", kc);
    //    Debug.Log(controls.Jump.Bindings.Count);
    //}
    //public void OnClickKeyBindUP()
    //{
    //    if (!upKeyBinding.IsActive())
    //        ActivationOfButtons(upKeyBinding.gameObject, true);
    //}
    //public void UpKeyBind()
    //{
    //    Key kc = (Key)System.Enum.Parse(typeof(KeyCode), upKeyBinding.text);
    //    controls.Up.ClearBindings();
    //    controls.Up.AddDefaultBinding(kc);
    //    playerActionDictionary.Add("Up", kc); ;
    //    SaveKeyBindings("Up", kc);
    //    Debug.Log(controls.Up.Bindings.Count);
    //}
    //public void OnClickKeyBindDown()
    //{
    //    if (!downKeyBinding.IsActive())
    //        ActivationOfButtons(downKeyBinding.gameObject, true);
    //}
    //public void DownKeyBind()
    //{
    //    Key kc = (Key)System.Enum.Parse(typeof(KeyCode), downKeyBinding.text);
    //    controls.Down.ClearBindings();
    //    controls.Down.AddDefaultBinding(kc);
    //    playerActionDictionary.Add("Down", kc); ;
    //    SaveKeyBindings("Down", kc);
    //    Debug.Log(controls.Down.Bindings.Count);
    //}
    //public void OnClickKeyBindLookLeft()
    //{
    //    if (!lookLeftKeyBinding.IsActive())
    //        ActivationOfButtons(lookLeftKeyBinding.gameObject, true);
    //}
    //public void LookLeftKeyBind()
    //{
    //    Key kc = (Key)System.Enum.Parse(typeof(KeyCode), lookLeftKeyBinding.text);
    //    controls.LookLeft.ClearBindings();
    //    controls.LookLeft.AddDefaultBinding(kc);
    //    playerActionDictionary.Add("Look Left", kc); ;
    //    SaveKeyBindings("Look Left", kc);
    //    Debug.Log(controls.LookLeft.Bindings.Count);
    //}
    //public void OnClickKeyBindingLookRight()
    //{
    //    if (!lookRightKeyBinding.IsActive())
    //        ActivationOfButtons(lookRightKeyBinding.gameObject, true);
    //}
    //public void LookRightKeyBind()
    //{
    //    Key kc = (Key)System.Enum.Parse(typeof(KeyCode), lookRightKeyBinding.text);
    //    controls.LookRight.ClearBindings();
    //    controls.LookRight.AddDefaultBinding(kc);
    //    playerActionDictionary.Add("Look Right", kc); ;
    //    SaveKeyBindings("Look Righ", kc);
    //    Debug.Log(controls.LookRight.Bindings.Count);
    //}
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
        //Debug.Log(QualitySettings.antiAliasing.ToString());
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
            ActivationOfButtons(audioSliders, true);
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
    }
    public void AmbientOcclusionChange()
    {
        postProcs.ambientOcclusion.enabled = !postProcs.ambientOcclusion.enabled;
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
        //PlayerPrefs.SetFloat("EventSFXVolume", sfxVolumeSlider.value);
        mixer.SetFloat("GeneralSFXVolume", sfxVolumeSlider.value);
        //PlayerPrefs.SetFloat("GeneralSFXVolume", sfxVolumeSlider.value);
        mixer.SetFloat("GunSoundsVolume", sfxVolumeSlider.value);
        //PlayerPrefs.SetFloat("GunSoundsVolume", sfxVolumeSlider.value);
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
            File.WriteAllText(_path + ".csv", "vSync: " + gameSettings.vSync.ToString() +"\n" + "Rotation Sensitivity: " + rotationSpeedSlider.value.ToString() + "\n" + "Texture Quality: " + gameSettings.textureQuality.ToString() + "\n" + "Anti-aliasing: " + gameSettings.antialiasing.ToString() + "\n" + "Resolution Index: " + gameSettings.resolutionIndex.ToString());
        }
        sw.Close();
    }
    public void GoBack(int _backSub)
    {
        backspacingIndex += _backSub;
        switch (backspacingIndex)
        {
            case 0:
                inUse = false;
                ActivationOfButtons(mainMenuBtns, true);
                ActivationOfButtons(mainOptionsSubBtns, false);
                ActivationOfButtons(displayMenuBtns, false);
                ActivationOfButtons(controlOptionsBtns, false);
                ActivationOfButtons(menuPanel, false);
                break;
            case 1:
                inUse = false;
                ActivationOfButtons(mainMenuBtns, false);
                ActivationOfButtons(mainOptionsSubBtns, true);
                ActivationOfButtons(displayMenuBtns, false);
                ActivationOfButtons(audioSliders, false);
                ActivationOfButtons(controlOptionsBtns, false);
                ActivationOfButtons(creditDisplay, false);
                ActivationOfButtons(menuPanel, false);
                break;
                //case 2:
                //    ActivationOfButtons(mainMenuBtns, true);
                //    ActivationOfButtons(mainOptionsSubBtns, false);
                //    ActivationOfButtons(displayMenuBtns, false);
                //    ActivationOfButtons(controlOptionsBtns, false);
                //    break;
                //case 3:

                //    break;
        }
    }
    public void ActivationOfButtons(GameObject _go, bool _activation)
    {
        _go.SetActive(_activation);
    }
    public void ActivationOfButtons(GameObject[] _btns, bool _activation)
    {
        Debug.Log(_btns[0].name.ToString());
        foreach (GameObject _btn in _btns)
            _btn.SetActive(_activation);
        //Debug.Log(_btns[0].gameObject.name.ToString());
    }
    public void ActivationOfButtons(List<Slider> _btns, bool _activation)
    {
        foreach (Slider _btn in _btns)
            _btn.gameObject.SetActive(_activation);
    }
}
