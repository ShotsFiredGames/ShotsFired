using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum WeaponType
    {
        Hitscan,
        Projectile,
        Sustained
    };

    public WeaponType weaponType;

    public GameObject thirdPersonGun;
    public string gunName;
    public int damage;
    public int ammo;
    public float fireFreq;
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

    // Projectile Variables
    public Projectile projectile;
    public double speed;
    public GameObject gunbarrel;
    public GameObject thirdPersonGunBarrel;
    public GameObject thirdPersonMuzzle;

    // Hitscan Variables
    public GameObject bulletHole;

    public GameObject muzzleFlash;

    //public void Overcharge();

    void Start()
    {
        SetAmmo();
        FindThirdPersonInfo();
        print(thirdPersonGunBarrel + " " + thirdPersonMuzzle);
    }

    public void FindThirdPersonInfo()
    {
        if (thirdPersonMuzzle != null)
        {
            thirdPersonGunBarrel = thirdPersonGun.transform.Find("GunBarrel").gameObject;
            thirdPersonMuzzle = thirdPersonGun.transform.Find("MuzzleFlash").gameObject;
            thirdPersonMuzzle.SetActive(false);
        }
    }

    public void SetAmmo()
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
            playerManager.CmdDisarm();
        }
    }

    public void SetActiveGun(bool isActive)
    {
        if (playerManager.isActiveAndEnabled)
        {
            gameObject.SetActive(isActive);
        }
        else
        {
            thirdPersonGun.SetActive(isActive);
        }
    }
}
