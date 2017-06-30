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

    [Header("General Sound Clips")]
    public AudioClip[] startMatch;
    public AudioClip[] endMatch;

    [Header("Nonevent Death Clips")]
    public AudioClip[] killedByPlayer;
    public AudioClip[] killedByEnvironment;

    [Header("Capture the Flag")]
    public AudioClip[] CTFStart;
    public AudioClip[] CTFEnd;
    public AudioClip[] flagCaptured;
    public AudioClip[] flagDropped;
    public AudioClip[] flagPickedUp;

    [Header("Reaper Sound Clips")]
    public AudioClip[] reaperStart;
    [Tooltip("Announcer says when Reaper kills")]
    public AudioClip[] reaperKill;
    [Tooltip("Announcer says when Reaper dies")]
    public AudioClip[] reaperDeath;
    public AudioClip[] reaperEndWithNoKills;
    public AudioClip[] reaperEndWithAKill;
    public AudioClip[] reaperEndPositiveKD;

	// Use this for initialization
	void Start ()
    {
        PlayRandomClipFromArray(startMatch);
	}

    void PlayRandomClipFromArray(AudioClip[] clips)
    {
        if (source.isPlaying)
            source.Stop();

        source.clip = clips[Random.Range(0, clips.Length)];

        if (source.clip != null)
            source.Play();
    }

    public void PlayDeathClip(string killer)
    {
        if (killer.Equals("Reaper"))
            PlayRandomClipFromArray(reaperKill);
        else if (killer.Equals("Environment"))
            PlayRandomClipFromArray(killedByEnvironment);
        else
            PlayRandomClipFromArray(killedByPlayer);
    }

    public void PlayKillingClip(string victim)
    {
        if (victim.Equals("Reaper"))
            PlayRandomClipFromArray(reaperDeath);
    }

    public void PlayEndGameClip()
    {
        PlayRandomClipFromArray(endMatch);
    }

    public void PlayStartEventClip(string eventName)
    {
        switch (eventName)
        {
            case "TheReaperComes":
                PlayRandomClipFromArray(reaperStart);
                break;
            case "CaptureTheFlag":
                PlayRandomClipFromArray(CTFStart);
                break;
        }
    }

    public void PlayReaperEndClip(double killRatio)
    {
        if (killRatio > 1) //positive kill death ratio (e.g 7/4)
            PlayRandomClipFromArray(reaperEndPositiveKD);
        else if (killRatio > 0) //got a kill atleast (e.g  2/4)
            PlayRandomClipFromArray(reaperEndWithAKill);
        else //got no kills (e.g  0/5)
            PlayRandomClipFromArray(reaperEndWithNoKills);
    }

    public void PlayCTFEndClip()
    {
        PlayRandomClipFromArray(CTFEnd);
    }

    public void PlayFlagCapturedClip()
    {
        PlayRandomClipFromArray(flagCaptured);
    }

    public void PlayFlagSeizedState(bool isHeld)
    {
        if (isHeld)
            PlayRandomClipFromArray(flagPickedUp);
        else
            PlayRandomClipFromArray(flagDropped);
    }


}
