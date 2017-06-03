using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ProjectileGun : Gun
{
    public Projectile projectile;
    public PlayerManager playerManager;
    public double speed;

    [Client]
    public override IEnumerator Fire()
    {
        if (!isFiring)
        {
            isFiring = true;
            Projectile bullet = projectile.GetPooledInstance<Projectile>();

            if (bullet == null)
            {
                yield break;
            }
            bullet.transform.position = spawnPoint.position;
            bullet.transform.rotation = transform.rotation;

            bullet.SetSpeed(speed);
            CmdStartMuzzleFlash();
            UseAmmo();

            yield return new WaitForSeconds(fireFreq);
            isFiring = false;
        } 
        
    }

    public override void UseAmmo()
    {
        if (isAmmoUnlimited) return;

        currentAmmo--;

        if (currentAmmo <= 0)
        {
            Discard();
        }
    }

    public override void Discard()
    {
        playerManager.Disarm();
    }
}
