using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    public enum WeaponType
    {
        Hitscan,
        Projectile,
        Sustained
    };

    public bool canAim;
    public WeaponType weaponType;

    public GameObject thirdPersonGun;
    public string gunName;
    public int damage;
    public int ammo;
    public float fireFreq;
    public Transform spawnPoint;
    public Vector3 basePosition;
    public Vector3 aimPosition;
    public AudioClip shootingSound;
    public GameObject muzzleFlash;

    public PlayerManager playerManager;

    [HideInInspector]
    public bool isFiring = false;

    [HideInInspector]
    public bool isAmmoUnlimited;
    [HideInInspector]
    public int currentAmmo;

    public GameObject projectile;
    public float speed;
    public GameObject gunbarrel;
    public GameObject thirdPersonGunBarrel;
    public GameObject thirdPersonMuzzle;


    //public abstract IEnumerator Fire();

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
        if (currentAmmo <= 0)
        {
            Discard();
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
