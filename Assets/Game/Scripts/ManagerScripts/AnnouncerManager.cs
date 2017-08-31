using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class AnnouncerManager : MonoBehaviour
{
    //Version of instance taken from "http://wiki.unity3d.com/index.php/AManagerClass"
    private static AnnouncerManager s_Instance = null;
    public static AnnouncerManager instance
    {
        get
        {
            if (s_Instance == null)
            {
                // This is where the magic happens.
                //  FindObjectOfType(...) returns the first AManager object in the scene.
                s_Instance = FindObjectOfType(typeof(AnnouncerManager)) as AnnouncerManager;
            }

            // If it is still null, create a new instance
            if (s_Instance == null)
            {
                GameObject obj = new GameObject("AnnouncerManager");
                s_Instance = obj.AddComponent(typeof(AnnouncerManager)) as AnnouncerManager;
                Debug.Log("Could not locate an AnnouncerManager object. AnnouncerManager was Generated Automaticly.");
            }

            return s_Instance;
        }
    }

    public PhotonView PhotonView { get; private set; }

    public AudioSource source;

    public GeneralClips generalClips;

    //Event Clips//
    public EventClips reaperClips;
    public SnatchNDashClips sndClips;
    public BallToTheWallClips bttwClips;
    public EventClips graspClips;
    public EventClips peckClips;
    public EventClips jammerClips;

    List<EventClips> eventClips = new List<EventClips>();

    //======//

    public AddOnClips addOnClips;
    public FillerClips fillerClips;

    void Awake()
    {
        PhotonView = GetComponent<PhotonView>();

        eventClips.Add(reaperClips);
        eventClips.Add(sndClips);
        eventClips.Add(graspClips);
        eventClips.Add(peckClips);
        eventClips.Add(jammerClips);
    }

    void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (PhotonNetwork.isMasterClient)
        {
            int arrayIndex = GetRandomIndex(generalClips.GetStartMatchClipArray(sceneName).Length);
            Local_PlayStartMatchClip(sceneName, arrayIndex);
            PhotonView.RPC("RPC_PlayStartMatchClip", PhotonTargets.Others, sceneName, arrayIndex);
        }
    }

    public int GetRandomIndex(int maxVal)
    {
        return Random.Range(0, maxVal);
    }

    void PlayRandomClipFromArray(AudioClip[] clips, int arrayIndex)
    {
        if (source.isPlaying)
            source.Stop();        

        source.clip = clips[arrayIndex];

        if (source.clip != null)
            source.Play();
    }

    void Local_PlayStartMatchClip(string sceneName, int arrayIndex)
    {
        AudioClip[] startMatch = generalClips.GetStartMatchClipArray(sceneName);
        PlayRandomClipFromArray(startMatch, arrayIndex);
    }

    [PunRPC]
    void RPC_PlayStartMatchClip(string sceneName, int arrayIndex)
    {
        AudioClip[] startMatch = generalClips.GetStartMatchClipArray(sceneName);
        PlayRandomClipFromArray(startMatch, arrayIndex);
    }

    public void Local_PlayEndMatchClip(int arrayIndex)
    {
        PlayRandomClipFromArray(generalClips.endMatch, arrayIndex);
    }

    [PunRPC]
    public void RPC_PlayEndMatchClip(int arrayIndex)
    {
        PlayRandomClipFromArray(generalClips.endMatch, arrayIndex);
    }

    #region Event Methods

    public int GetRandomEventIndex(string eventName)
    {
        foreach (EventClips eC in eventClips)
        {
            if (eC.eventName.Equals(eventName))
                return GetRandomIndex(eC.eventStart.Length);
        }

        Debug.LogError("Could not find event: " + eventName + ". Did you forget to add it to the AnnouncerManager PlayEventStartClip method?");
        return 0;
    }

    public void Local_PlayEventStartClip(string eventName, int arrayIndex)
    {
        PlayEventStartingClip(eventName, arrayIndex);
    }

    [PunRPC]
    public void RPC_PlayEventStartClip(string eventName, int arrayIndex)
    {
        PlayEventStartingClip(eventName, arrayIndex);
    }

    void PlayEventStartingClip(string eventName, int arrayIndex)
    {
        foreach (EventClips eC in eventClips)
        {
            if (eC.eventName.Equals(eventName))
            {
                PlayRandomClipFromArray(eC.eventStart, arrayIndex);
                Debug.LogError("Event Name for Start Clip: " + eventName);
            }               
        }
    }

    public int GetRandomAddOnIndex(string addOnName)
    {
        switch (addOnName)
        {
            case "HugeLoad":
                return GetRandomIndex(addOnClips.hugeLoadClips.Length);
            case "MimicAppears":
                return GetRandomIndex(addOnClips.mimicClips.Length);
            case "BombsAway":
                return GetRandomIndex(addOnClips.bombsAwayClips.Length);
            case "StepItUp":
                return GetRandomIndex(addOnClips.stepItUpClips.Length);
            case "WeAllFloatOn":
                return GetRandomIndex(addOnClips.allFloatOnClips.Length);
            case "NeedHealing":
                return GetRandomIndex(addOnClips.needHealingClips.Length);
            case "ShotsAllAround":
                return GetRandomIndex(addOnClips.shotsAllAroundClips.Length);
            default:
                Debug.LogError("Could not find add-on: " + addOnName + ". Did you forget to add it to the AnnouncerManager PlayAddOnStartClip method?");
                return 0;
        }
    }

    public void Local_PlayAddOnStartClip(string addOnName, int arrayIndex)
    {
        PlayAddOnStartClip(addOnName, arrayIndex);
    }

    [PunRPC]
    public void RPC_PlayAddOnStartClip(string addOnName, int arrayIndex)
    {
        PlayAddOnStartClip(addOnName, arrayIndex);
    }

    void PlayAddOnStartClip(string addOnName, int arrayIndex)
    {
        switch (addOnName)
        {
            case "HugeLoad":
                PlayRandomClipFromArray(addOnClips.hugeLoadClips, arrayIndex);
                break;
            case "MimicAppears":
                PlayRandomClipFromArray(addOnClips.mimicClips, arrayIndex);
                break;
            case "BombsAway":
                PlayRandomClipFromArray(addOnClips.bombsAwayClips, arrayIndex);
                break;
            case "StepItUp":
                PlayRandomClipFromArray(addOnClips.stepItUpClips, arrayIndex);
                break;
            case "WeAllFloatOn":
                PlayRandomClipFromArray(addOnClips.allFloatOnClips, arrayIndex);
                break;
            case "NeedHealing":
                PlayRandomClipFromArray(addOnClips.needHealingClips, arrayIndex);
                break;
            case "ShotsAllAround":
                PlayRandomClipFromArray(addOnClips.shotsAllAroundClips, arrayIndex);
                break;
            default:
                Debug.LogError("Could not find add-on: " + addOnName + ". Did you forget to add it to the AnnouncerManager PlayAddOnStartClip method?");
                break;
        }
    }


    #endregion

    #region Clip Classes
    [System.Serializable]
    public class GeneralClips
    {
        public StartMatchClips scionArena;
        public AudioClip[] endMatch;

        public AudioClip[] GetStartMatchClipArray(string sceneName)
        {
            switch (sceneName)
            {
                case "Game":
                    return scionArena.startMatch;
                default:
                    return null;
            }
        }
    }

    [System.Serializable]
    public class StartMatchClips
    {
        public AudioClip[] startMatch;
    }

    [System.Serializable]
    public class EventClips
    {
        public string eventName;
        public AudioClip[] eventStart;
        public AudioClip[] eventDescription;
        public AudioClip[] eventEnd;
    }

    [System.Serializable]
    public class SnatchNDashClips : EventClips
    {
        public AudioClip[] sndMostFlagsCapped;
        public AudioClip[] sndNoFlagsCapped;
    }

    [System.Serializable]
    public class BallToTheWallClips : EventClips
    {
        public AudioClip[] bttwMostGoals;
    }

    [System.Serializable]
    public class AddOnClips
    {
        public AudioClip[] hugeLoadClips;
        public AudioClip[] bombsAwayClips;
        public AudioClip[] mimicClips;
        public AudioClip[] shotsAllAroundClips;
        public AudioClip[] needHealingClips;
        public AudioClip[] stepItUpClips;
        public AudioClip[] allFloatOnClips;
    }

    [System.Serializable]
    public class FactionClips
    {
        public AudioClip[] firstBlood;
        public AudioClip[] killStreaks;
    }

    [System.Serializable]
    public class FillerClips
    {
        public FactionClips redFactionClips;
        public FactionClips blueFactionClips;
        public FactionClips whiteFactionClips;
        public FactionClips greenFactionClips;
        public FactionClips blackFactionClips;
        public FactionClips yellowFactionClips;
        public AudioClip[] randomLines;
    }
    #endregion
}