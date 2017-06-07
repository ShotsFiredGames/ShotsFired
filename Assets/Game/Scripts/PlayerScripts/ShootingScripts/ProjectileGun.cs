using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ProjectileGun : Gun
{
    public GameObject gunbarrel;
    public Projectile projectile;
    public LayerMask layermask;

    GameObject myCamera;
    public double speed;
    RaycastHit hit;

    public void Start()
    {
        SetAmmo();
        shooting = transform.root.GetComponent<Shooting>();
        myCamera = transform.root.Find("Main Camera").gameObject;
    }

    [Client]
    public override IEnumerator Fire()
    {
        if (!isFiring)
        {
            isFiring = true;
            if (Physics.Raycast(myCamera.transform.position, myCamera.transform.forward, out hit, 1000, layermask))
            {
                shooting.CmdSpawnProjectile(gunbarrel.transform.position, transform.root.rotation, hit.point, speed, 0);
                //CmdStartMuzzleFlash();
                UseAmmo();

                yield return new WaitForSeconds(fireFreq);
                isFiring = false;
            }
        } 
    }

    public override void Discard()
    {
        playerManager.CmdDisarm();
    }
}
