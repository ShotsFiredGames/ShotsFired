using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class Gun : NetworkBehaviour
{
    public GameObject thirdPersonGun;
    public string gunName;
    public int damage;
    public int ammo;
    public float fireFreq;
    public GameObject muzzleFlash;
    public Transform spawnPoint;
    public Vector3 basePosition;
    public Vector3 aimPosition;

    public PlayerManager playerManager;

    [HideInInspector]
    public bool isFiring = false;

    [HideInInspector]
    public bool isAmmoUnlimited;
    [HideInInspector]
    public int currentAmmo;


	public abstract IEnumerator Fire();

    public abstract void Discard();
  
    //public void Overcharge();

    public void SetAmmo()
    {
        this.isAmmoUnlimited = GameCustomization.isAmmoUnlimited;
        currentAmmo = ammo;
    }

    public void UseAmmo()
    {
        if (isAmmoUnlimited) return;

        currentAmmo--;
        print("Shooting: " + currentAmmo);
        if (currentAmmo <= 0)
        {
            Discard();
        }
    }

    public void SetActiveGun(bool isActive)
    {
        if (playerManager.isActiveAndEnabled)
        {
            Debug.LogError("is local");
            gameObject.SetActive(isActive);
        }
        else
        {
            Debug.LogError("is not local");
            thirdPersonGun.SetActive(isActive);
        }
           
    }

    [Command]
    public void CmdStartMuzzleFlash()
    {
        if (muzzleFlash == null) return;
        RpcStartMuzzleFlash();
    }

    [ClientRpc]
    void RpcStartMuzzleFlash()
    {
        StartCoroutine(MuzzleFlash());                                                                  //Activate the MuzzleFlash
    }

    IEnumerator MuzzleFlash()                                                                                   //Activate and DeActivate the muzzle flash
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(.05f);
        muzzleFlash.SetActive(false);
    }

}
