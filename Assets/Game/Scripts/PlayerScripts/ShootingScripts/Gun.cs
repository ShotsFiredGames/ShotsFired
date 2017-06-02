using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class Gun : NetworkBehaviour
{
    public string gunName;
    public int damage;
    public int ammo;
    public float fireFreq;
    public GameObject muzzleFlash;
    public Transform spawnPoint;

    [HideInInspector]
    public bool isFiring = false;

    bool isAmmoUnlimited;

    int currentAmmo;

	public abstract IEnumerator Fire();

    public void Discard()
    {
        //PlayerManager.SetState("Disarmed");
    }
    //public void Overcharge();

    void Start()
    {
        this.isAmmoUnlimited = GameCustomization.isAmmoUnlimited;
        currentAmmo = ammo;
    }

    public void UseAmmo()
    {
        if (isAmmoUnlimited) return;

        currentAmmo--;

        if (currentAmmo <= 0)
        {
            Discard();
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
