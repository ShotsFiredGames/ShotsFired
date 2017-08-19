using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFlagInfo : MonoBehaviour
{
    public bool hasFlag { get; set; }
    public Image haveFlag;
    public Transform carriedFlagPosition;

    PlayerHealth health;

    void Start()
    {
        health = GetComponent<PlayerHealth>();
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        haveFlag.gameObject.SetActive(hasFlag);
    }

    public void DropFlag()
    {
        if (hasFlag)
            FlagManager.instance.photonView.RPC("RPC_FlagDropped", PhotonTargets.All, name);
    }

    public Vector3 GetCarriedFlagPostion()
    {
        return carriedFlagPosition.position;
    }

    public bool CheckAbilityToPickupFlag()
    {
        if (health.isDead)
            return false;

        if (hasFlag)
            return false;

        return true;
    }
}
