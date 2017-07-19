using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagBase : MonoBehaviour
{
    public GameObject flag;
    public PlayerManager owner;
    public bool hasFlag = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Flag")) // if a flag enters the base
        {
            Debug.LogError("Flag returned");
            if (hasFlag) //if the base has its flag
            {
                Debug.LogError("Base has a flag");
                if (!other.gameObject.name.Equals(flag.name)) //if the flag isn't the flag the base owns
                {
                    Debug.LogError("Flag reset");
                    FlagManager.instance.CmdReturnFlag(other.GetComponent<Flag>().index);
                    hasFlag = false;
                }                
            }            
        }
    }


}
