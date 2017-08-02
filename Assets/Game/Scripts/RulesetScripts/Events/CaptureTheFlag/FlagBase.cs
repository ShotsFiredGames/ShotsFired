﻿using UnityEngine;

public class FlagBase : MonoBehaviour
{
    public Flag flag;
    public PlayerManager owner;
    public bool hasFlag = true;

    void Start()
    {
        hasFlag = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (owner == null) return; //if they don't have an owner, they cannot be used to capture a flag
        if (other.tag.Equals("Flag")) // if a flag enters the base
        {
            Flag capturedFlag = other.GetComponent<Flag>();
            Debug.LogError("Flag returned: " + hasFlag);
            if (hasFlag) //if the base has its flag
            {
                Debug.LogError("Base has a flag that belongs to: " + owner.name);
                Debug.LogError("Flag being captured is: " + capturedFlag.name);
                if (!capturedFlag.index.Equals(flag.index)) //if the flag isn't the flag the base owns
                {
                    Debug.LogError("Flag reset");
                    FlagManager.instance.photonView.RPC("RPC_ReturnFlag", PhotonTargets.All, other.GetComponent<Flag>().index);
                    hasFlag = false;
                }
            }
        }
    }


}
