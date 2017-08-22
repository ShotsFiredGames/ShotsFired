using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RefereeManager : MonoBehaviour
{
    #region Instance
    //Version of instance taken from "http://wiki.unity3d.com/index.php/AManagerClass"
    private static RefereeManager s_Instance = null;
    public static RefereeManager instance
    {
        get
        {
            if (s_Instance == null)
            {
                // This is where the magic happens.
                //  FindObjectOfType(...) returns the first AManager object in the scene.
                s_Instance = FindObjectOfType(typeof(RefereeManager)) as RefereeManager;
            }

            // If it is still null, create a new instance
            if (s_Instance == null)
            {
                GameObject obj = new GameObject("RefereeManager");
                s_Instance = obj.AddComponent(typeof(RefereeManager)) as RefereeManager;
                Debug.Log("Could not locate an RefereeManager object. RefereeManager was Generated Automaticly.");
            }

            return s_Instance;
        }
    }
    #endregion

    public ReaperClips reaperClips;
    public SnatchNDashClips sndClips;
    public BallToTheWallClips bttwClips;

    AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    void PlayRandomClipFromArray(AudioClip[] clips, int arrayIndex)
    {
        if (source.isPlaying)
            source.Stop();

        source.clip = clips[arrayIndex];

        if (source.clip != null)
            source.Play();
    }

    public int GetRandomIndex(int maxVal)
    {
        return Random.Range(0, maxVal);
    }

    #region Event Methods
    public void PlayReaperDies()
    {
        int randomInt = GetRandomIndex(reaperClips.reaperDies.Length);
        PlayRandomClipFromArray(reaperClips.reaperDies, randomInt);
    }

    public void PlayGoalScored()
    {
        int randomInt = GetRandomIndex(bttwClips.goalScored.Length);
        PlayRandomClipFromArray(bttwClips.goalScored, randomInt);
    }

    public void PlayFlagStolen()
    {
        int randomInt = GetRandomIndex(sndClips.flagStolen.Length);
        PlayRandomClipFromArray(sndClips.flagStolen, randomInt);
    }

    public void PlayFlagDropped()
    {
        int randomInt = GetRandomIndex(sndClips.flagDropped.Length);
        PlayRandomClipFromArray(sndClips.flagDropped, randomInt);
    }

    public void PlayFlagReturned()
    {
        int randomInt = GetRandomIndex(sndClips.flagReturned.Length);
        PlayRandomClipFromArray(sndClips.flagReturned, randomInt);
    }

    public void PlayFlagPickedUp()
    {
        int randomInt = GetRandomIndex(sndClips.flagPickedUp.Length);
        PlayRandomClipFromArray(sndClips.flagPickedUp, randomInt);
    }
    #endregion

    #region Classes
    [System.Serializable]
    public class ReaperClips
    {
        public AudioClip[] reaperDies;
    }

    [System.Serializable]
    public class BallToTheWallClips
    {
        public AudioClip[] goalScored;
    }

    [System.Serializable]
    public class SnatchNDashClips
    {
        public AudioClip[] flagStolen;
        public AudioClip[] flagDropped;
        public AudioClip[] flagReturned;
        public AudioClip[] flagPickedUp;
    }
    #endregion
}
