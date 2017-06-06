using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ProjectileGun : Gun
{
    public Projectile projectile;
    public double speed;

    public void Start()
    {
        SetAmmo();
        shooting = transform.root.GetComponent<Shooting>();
    }

    public override IEnumerator Fire()
    {
        if (!isFiring)
        {
            isFiring = true;
            shooting.CmdSpawnProjectile(transform.position, transform.rotation, transform.up, speed, 0);
            //CmdStartMuzzleFlash();
            UseAmmo();

            yield return new WaitForSeconds(fireFreq);
            isFiring = false;
        } 
        
    }

    public override void Discard()
    {
        playerManager.CmdDisarm();
    }
}
