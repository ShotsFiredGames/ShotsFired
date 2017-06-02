using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HitscanGun : Gun
{
    public GameObject bulletHole;
    public LayerMask layermask;
    RaycastHit hit;

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
                    Debug.LogError(hit.transform.parent.transform.parent.name);
                    //CmdPlayerShot(hit.transform.parent.transform.parent.name);
                }
                else
                {
                    Vector3 position = hit.point + (hit.normal * .1f);
                    Quaternion rotation = Quaternion.LookRotation(hit.normal);
                    CmdBulletHole(position, rotation);                                                                    //Spawn the bullet hole where the raycast hit.
                }
            }

            UseAmmo();

            yield return new WaitForSeconds(fireFreq);
            isFiring = false;
        }
    }

    [Command]
    void CmdBulletHole(Vector3 position, Quaternion rotation)
    {
        Debug.LogError("Hit or something so we know it is working");
        if (bulletHole == null) return;
        GameObject hole = Instantiate(bulletHole, position, rotation) as GameObject;
        NetworkServer.Spawn(hole);
    }
}
