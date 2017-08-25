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
        GameManager.instance.PlayerGainsPoints(player, pointsForCapture);
    }

    public void FlagHeld(string player)
    {
        GameManager.instance.PlayerGainsPoints(player, pointsForHolding);
    }

    public byte GetFlagNumber()
    {
        return flagNumber++;
    }

    [PunRPC]
    void RPC_FlagPickedUp(byte flagNum, string carrierName)
    {
        Flag flag = ConvertFlagFromIndex(flagNum);

        if (flag.resetTimer != null)
            StopCoroutine(flag.resetTimer);

        if (flag.carrier != null)
            flag.carrier.hasFlag = false;

        if (flag.flagBase != null)
        {
            if (flag.flagBase.owner.name.Equals(PhotonNetwork.player.NickName)) // if the local player is the owner of the flag being picked up
                RefereeManager.instance.PlayFlagStolen();
        }

        PlayerManager newCarrier = PlayerWrangler.GetPlayer(carrierName);

        if (newCarrier == null)
            return;

        if (!newCarrier.CheckAbilityToPickupFlag())
            return;

        PlayerFlagInfo flagInfo = newCarrier.GetComponent<PlayerFlagInfo>();         
        flag.carrier = flagInfo;

        if (flag.GetStringOfCarrier().Equals(PhotonNetwork.player.NickName)) // if the local player is who picked up the flag
            RefereeManager.instance.PlayFlagPickedUp();

        flagInfo.hasFlag = true;
        flag.transform.SetParent(flagInfo.transform);
        flag.transform.position = flagInfo.GetCarriedFlagPostion();

        if (flag.flagBase != null)
            flag.flagBase.hasFlag = false;
    }

    [PunRPC]
    void RPC_ReturnFlag(byte flagNum)
    {
        Flag flag = ConvertFlagFromIndex(flagNum);

        if (flag.carrier != null)
        {
            Debug.LogError("there was a carrier : " + flag.carrier.name);
            flag.carrier.hasFlag = false;
            if (flag.GetStringOfCarrier().Equals(PhotonNetwork.player.NickName)) // if the local player is who returned the flag
                RefereeManager.instance.PlayFlagCaptured();
        }

        if (flag.flagBase.owner != null)
        {
            if (flag.flagBase.owner.name.Equals(PhotonNetwork.player.NickName)) // if the local player is who returned the flag
                RefereeManager.instance.PlayFlagReturned();
        }


        if (PhotonNetwork.isMasterClient)
        	FlagReturned(flag.GetStringOfCarrier());

        flag.ResetFlagPosition();
        StartCoroutine(flag.CanBePickedUp());                    
    }

    [PunRPC]
    public void RPC_FlagDropped(string owner)
    {
        Flag flag = ConvertFlagFromPlayerName(owner);

        if (flag == null)
            return;

        if (flag.carrier != null)
        {
            flag.carrier.hasFlag = false;

            if (flag.GetStringOfCarrier().Equals(PhotonNetwork.player.NickName)) // if the local player is who returned up the flag
                RefereeManager.instance.PlayFlagDropped();
        }            

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