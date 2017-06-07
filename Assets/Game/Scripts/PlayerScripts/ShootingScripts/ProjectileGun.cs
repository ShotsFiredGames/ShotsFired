using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileGun : Gun
{
    public Projectile projectile;
    public double speed;

    //public override IEnumerator Fire()
    //{
    //    if (!isFiring)
    //    {
    //        isFiring = true;
    //        Projectile bullet = projectile.GetPooledInstance<Projectile>();

    //        if (bullet == null)
    //        {
    //            yield break;
    //        }
    //        bullet.transform.position = spawnPoint.position;
    //        bullet.transform.rotation = transform.rotation;

    //        bullet.SetSpeed(speed);
    //        //CmdStartMuzzleFlash();
    //        UseAmmo();

    //        yield return new WaitForSeconds(fireFreq);
    //        isFiring = false;
    //    } 
        
    //}

    public override void Discard()
    {
        playerManager.CmdDisarm();
    }
}
