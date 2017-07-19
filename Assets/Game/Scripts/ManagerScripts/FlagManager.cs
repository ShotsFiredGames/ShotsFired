using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FlagManager : NetworkBehaviour
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
    [SyncVar]
    public byte flagNumber = 0;
    public List<Flag> flags = new List<Flag>();

    public void FlagReturned(string player)
    {
        GameManager.instance.FlagCaptured(player, pointsForCapture);
    }

    public byte GetFlagNumber()
    {
        return flagNumber++;
    }

    [Command]
    public void CmdFlagPickedUp(byte flagNum)
    {
        RpcFlagPickedUp(flagNum);
    }

    [ClientRpc]
    void RpcFlagPickedUp(byte flagNum)
    {
        Flag flag = ConvertFlagFromIndex(flagNum);
        if (flag.resetTimer != null)
            StopCoroutine(flag.resetTimer);

        flag.carrier.GetComponent<PlayerManager>().hasFlag = true;
        flag.flagBase.hasFlag = false;
        flag.transform.SetParent(flag.carrier.transform);
        flag.transform.position = flag.carrier.transform.position + new Vector3(0, flag.carrier.transform.localScale.y, 0);

       // if (flagSource != null)
         //   flagSource.PlayOneShot(pickupClip);
    }

    [Command]
    public void CmdReturnFlag(byte flagNum)
    {
        RpcReturnFlag(flagNum);
    }

    [ClientRpc]
    void RpcReturnFlag(byte flagNum)
    {
        Flag flag = ConvertFlagFromIndex(flagNum);
        //if (flagSource != null)
            //flagSource.PlayOneShot(returnedClip);

        FlagReturned(flag.GetStringOfCarrier());
        StartCoroutine(flag.CanBePickedUp());
        flag.ResetFlagPosition();

        if (flag.carrier != null)
            flag.carrier.GetComponent<PlayerManager>().hasFlag = false;
    }

    [Command]
    public void CmdFlagDropped(byte flagNum)
    {
        RpcFlagDropped(flagNum);
    }

    [ClientRpc]
    public void RpcFlagDropped(byte flagNum)
    {
        Flag flag = ConvertFlagFromIndex(flagNum);
        if (flag.carrier != null)
            flag.carrier.GetComponent<PlayerManager>().hasFlag = false;

        flag.transform.parent = null;
        StartCoroutine(flag.CanBePickedUp());
        flag.resetTimer = StartCoroutine(flag.ResetTimer());
        //if (flagSource != null)
            //flagSource.PlayOneShot(dropClip);
    }

    Flag ConvertFlagFromIndex(byte index)
    {
        return flags[index];
    }
}
