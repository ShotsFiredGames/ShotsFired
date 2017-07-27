using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
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
 
    public AudioSource source;

    public GeneralClips generalClips;
    public ReaperClips reaperClips;
    public SnatchNDashClips sndClips;
    public BallToTheWallClips bttwClips;
    public AddOnClips addOnClips;

    [Header("Filler Lines")]
    public FactionClips[] factionClips;
    public FillerClips fillerClips;
 
    void Start()
    {
        PlayRandomClipFromArray(generalClips.startMatch);
    }
 
    void PlayRandomClipFromArray(AudioClip[] clips)
    {
        if (source.isPlaying)
            source.Stop();
 
        source.clip = clips[Random.Range(0, clips.Length)];
 
        if (source.clip != null)
            source.Play();
    }

    #region Clip Classes
    [System.Serializable]
    public class GeneralClips
    {
        public AudioClip[] startMatch;
        public AudioClip[] endMatch;
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
        public AudioClip testTwo;
    }

    [System.Serializable]
    public class FillerClips
    {
        public AudioClip[] randomLines;
    }
    #endregion
}