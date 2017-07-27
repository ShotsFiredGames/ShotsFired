using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
 
public class AnnouncerManager : NetworkBehaviour
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
 
    public AudioSource source;

    public GeneralClips generalClips;
    public ReaperClips reaperClips;
    public SnatchNDashClips sndClips;
    public BallToTheWallClips bttwClips;
    public AddOnClips addOnClips;
    public FillerClips fillerClips;

    [SyncVar]
    int arrayIndex;
 
    void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        AudioClip[] startMatch = generalClips.GetStartMatchClipArray(sceneName);
        PlayRandomClipFromArray(startMatch);
    }

    [ServerCallback]
    void GetRandomIndex(int maxVal)
    {
        arrayIndex = Random.Range(0, maxVal);
    }

    void PlayRandomClipFromArray(AudioClip[] clips)
    {
        if (source.isPlaying)
            source.Stop();

        GetRandomIndex(clips.Length);
        source.clip = clips[arrayIndex];
 
        if (source.clip != null)
            source.Play();
    }

    public void PlayEndMatchClip()
    {
        PlayRandomClipFromArray(generalClips.endMatch);
    }

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
    public class ReaperClips
    {
        public AudioClip[] reaperStart;
        public AudioClip[] reaperEnd;
    }

    [System.Serializable]
    public class SnatchNDashClips
    {
        public AudioClip[] sndStart;
        public AudioClip[] sndMostFlagsCapped;
        public AudioClip[] sndNoFlagsCapped;
        public AudioClip[] sndEnd;
    }

    [System.Serializable]
    public class BallToTheWallClips
    {
        public AudioClip[] bttwStart;
        public AudioClip[] bttwMostGoals;
        public AudioClip[] bttwEnd;
    }

    [System.Serializable]
    public class AddOnClips
    {
        public AudioClip[] hugeLoadClips;
        public AudioClip[] bombsAwayClips;
        public AudioClip[] mimicClips;
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