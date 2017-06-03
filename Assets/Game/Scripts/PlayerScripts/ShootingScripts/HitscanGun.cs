using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HitscanGun : Gun
{
    public GameObject bulletHole;
    public LayerMask layermask;
    public PlayerManager playerManager;
    RaycastHit hit;

    private void Start()
    {
        print("Shooting: " + currentAmmo);
    }
    public override IEnumerator Fire()
    {
        if (!isFiring)
        {
            isFiring = true;

            CmdStartMuzzleFlash();

            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 1000, layermask))
            {
                if (hit.transform.tag == "Collision")
                {
                    //CmdPlayerShot(hit.transform.parent.transform.parent.name);
                }
                else
                {
                    print("I hit something");
                    Vector3 position = hit.point + (hit.normal * .1f);
                    Quaternion rotation = Quaternion.LookRotation(hit.normal);
                    CmdBulletHole(position, rotation);

                    RpcPrint();
                }
            }

            UseAmmo();

            yield return new WaitForSeconds(fireFreq);
            isFiring = false;
        }
    }

    public override void UseAmmo()
    {
        if (isAmmoUnlimited) return;

        currentAmmo--;
        print("Shooting: " + currentAmmo);
        if (currentAmmo <= 0)
        {
            Discard();
        }
    }

    public override void Discard()
    {
        playerManager.Disarm();
    }

    [ClientRpc]
    void RpcPrint()
    {

        print("Spawn bullet hole");
    }

    [Command]
    void CmdBulletHole(Vector3 position, Quaternion rotation)
    {
        RpcPrint();
        GameObject hole = Instantiate(bulletHole, position, rotation) as GameObject;
        NetworkServer.Spawn(hole);
    }
}
