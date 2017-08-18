using UnityEngine;
using UnityEngine.SceneManagement;
 
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
    public ReaperClips reaperClips;
    public SnatchNDashClips sndClips;
    public BallToTheWallClips bttwClips;
    public AddOnClips addOnClips;
    public FillerClips fillerClips;

    void Awake()
    {
        PhotonView = GetComponent<PhotonView>();
    }

    void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (PhotonNetwork.isMasterClient)
        {
            int arrayIndex = GetRandomIndex(generalClips.GetStartMatchClipArray(sceneName).Length);
			Local_PlayStartMatchClip (sceneName, arrayIndex);
			PhotonView.RPC("RPC_PlayStartMatchClip", PhotonTargets.OthersBuffered, sceneName, arrayIndex);
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
        switch (eventName)
        {
            case "TheReaperComes":
                return GetRandomIndex(reaperClips.reaperStart.Length);
            case "SnatchNDash":
                return GetRandomIndex(sndClips.sndStart.Length);
            case "BallToTheWall":
                return GetRandomIndex(bttwClips.bttwStart.Length);
            default:
                Debug.LogError("Could not find event: " + eventName + ". Did you forget to add it to the AnnouncerManager PlayEventStartClip method?");
                return 0;
        }
    }

	public void Local_PlayEventStartClip(string eventName, int arrayIndex)
	{
		switch (eventName)
		{
		case "TheReaperComes":
			PlayRandomClipFromArray(reaperClips.reaperStart, arrayIndex);
			break;
		case "SnatchNDash":
			PlayRandomClipFromArray(sndClips.sndStart, arrayIndex);
			break;
		case "BallToTheWall":
			PlayRandomClipFromArray(bttwClips.bttwStart, arrayIndex);
			break;
		default:
			Debug.LogError("Could not find event: " + eventName + ". Did you forget to add it to the AnnouncerManager PlayEventStartClip method?");
			break;
		}
	}

    [PunRPC]
    public void RPC_PlayEventStartClip(string eventName, int arrayIndex)
    {
        switch (eventName)
        {
            case "TheReaperComes":
                PlayRandomClipFromArray(reaperClips.reaperStart, arrayIndex);
                break;
            case "SnatchNDash":
                PlayRandomClipFromArray(sndClips.sndStart, arrayIndex);
                break;
            case "BallToTheWall":
                PlayRandomClipFromArray(bttwClips.bttwStart, arrayIndex);
                break;
            default:
                Debug.LogError("Could not find event: " + eventName + ". Did you forget to add it to the AnnouncerManager PlayEventStartClip method?");
                break;
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
            default:
                Debug.LogError("Could not find add-on: " + addOnName + ". Did you forget to add it to the AnnouncerManager PlayAddOnStartClip method?");
                return 0;
        }
    }

	public void Local_PlayAddOnStartClip(string addOnName, int arrayIndex)
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
		default:
			Debug.LogError("Could not find add-on: " + addOnName + ". Did you forget to add it to the AnnouncerManager PlayAddOnStartClip method?");
			break;
		}
	}

    [PunRPC]
    public void RPC_PlayAddOnStartClip(string addOnName, int arrayIndex)
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