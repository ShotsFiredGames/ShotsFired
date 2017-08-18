using System.Collections.Generic;
using UnityEngine;

public class FlagManager : Photon.MonoBehaviour
{
    //Version of instance taken from "http://wiki.unity3d.com/index.php/AManagerClass"
    private static FlagManager s_Instance = null;
    public static FlagManager instance
    {
        get
        {
            if (s_Instance == null)
            {
                // This is where the magic happens.
                //  FindObjectOfType(...) returns the first AManager object in the scene.
                s_Instance = FindObjectOfType(typeof(FlagManager)) as FlagManager;
            }

            // If it is still null, create a new instance
            if (s_Instance == null)
            {
                GameObject obj = new GameObject("FlagManager");
                s_Instance = obj.AddComponent(typeof(FlagManager)) as FlagManager;
                Debug.Log("Could not locate an FlagManager object. FlagManager was Generated Automaticly.");
            }

            return s_Instance;
        }
    }

    public byte pointsForCapture { get; set; }
    public byte pointsForHolding { private get; set; }
    public byte flagNumber = 0;
    public List<Flag> flags = new List<Flag>();

    public void FlagReturned(string player)
    {
        GameManager.instance.FlagCaptured(player, pointsForCapture);
    }

    public void FlagHeld(string player)
    {
        GameManager.instance.FlagCaptured(player, pointsForHolding);
    }

    public byte GetFlagNumber()
    {
        return flagNumber++;
    }

	public void Local_FlagPickedUp(byte flagNum, string carrierName)
	{
		Flag flag = ConvertFlagFromIndex(flagNum);

		if (flag.resetTimer != null)
			StopCoroutine(flag.resetTimer);

		PlayerManager newCarrier = PlayerWrangler.GetPlayer(carrierName);

		if (!newCarrier.CheckAbilityToPickupFlag())
			return;

		flag.carrier = newCarrier;
		flag.carrier.hasFlag = true;
		flag.flagBase.hasFlag = false;
		flag.transform.SetParent(flag.carrier.transform);
		flag.transform.position = flag.carrier.transform.position + new Vector3(0, flag.carrier.transform.localScale.y, 0);

		if (flag.flagBase != null)
			flag.flagBase.hasFlag = false;
	}

    [PunRPC]
    void RPC_FlagPickedUp(byte flagNum, string carrierName)
    {
        Flag flag = ConvertFlagFromIndex(flagNum);

        if (flag.resetTimer != null)
            StopCoroutine(flag.resetTimer);

        PlayerManager newCarrier = PlayerWrangler.GetPlayer(carrierName);

        if (!newCarrier.CheckAbilityToPickupFlag())
            return;

        flag.carrier = newCarrier;
        flag.carrier.hasFlag = true;
        flag.flagBase.hasFlag = false;
        flag.transform.SetParent(flag.carrier.transform);
        flag.transform.position = flag.carrier.transform.position + new Vector3(0, flag.carrier.transform.localScale.y, 0);

        if (flag.flagBase != null)
            flag.flagBase.hasFlag = false;
    }

	public void Local_ReturnFlag(byte flagNum)
	{
		Flag flag = ConvertFlagFromIndex(flagNum);

		if(PhotonNetwork.isMasterClient)
			FlagReturned(flag.GetStringOfCarrier());
		StartCoroutine(flag.CanBePickedUp());
		flag.ResetFlagPosition();

		if (flag.carrier != null)
			flag.carrier.GetComponent<PlayerManager>().hasFlag = false;
	}

    [PunRPC]
    void RPC_ReturnFlag(byte flagNum)
    {
        Flag flag = ConvertFlagFromIndex(flagNum);

		if(PhotonNetwork.isMasterClient)
        	FlagReturned(flag.GetStringOfCarrier());
        StartCoroutine(flag.CanBePickedUp());
        flag.ResetFlagPosition();

        if (flag.carrier != null)
            flag.carrier.GetComponent<PlayerManager>().hasFlag = false;
    }

	public void Local_FlagDropped(string owner)
	{
		Flag flag = ConvertFlagFromPlayerName(owner);

		if (flag != null && flag.carrier != null)
			flag.carrier.GetComponent<PlayerManager>().hasFlag = false;

		flag.transform.parent = null;
		StartCoroutine(flag.CanBePickedUp());
		flag.resetTimer = StartCoroutine(flag.ResetTimer());
	}

    [PunRPC]
    public void RPC_FlagDropped(string owner)
    {
        Flag flag = ConvertFlagFromPlayerName(owner);

        if (flag != null && flag.carrier != null)
            flag.carrier.GetComponent<PlayerManager>().hasFlag = false;

        flag.transform.parent = null;
        StartCoroutine(flag.CanBePickedUp());
        flag.resetTimer = StartCoroutine(flag.ResetTimer());
    }

    Flag ConvertFlagFromIndex(byte index)
    {
        return flags[index];
    }

    Flag ConvertFlagFromPlayerName(string owner)
    {
        PlayerManager player = PlayerWrangler.GetPlayer(owner);

        foreach (Flag flag in flags)
        {
            if (flag.carrier != null)
            {
                if (flag.carrier.name.Equals(player.name))
                    return flag;
            }
        }

        return null;
    }
}