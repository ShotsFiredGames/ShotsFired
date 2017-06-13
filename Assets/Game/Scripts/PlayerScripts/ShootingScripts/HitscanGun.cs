using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HitscanGun : Gun
{
    public LayerMask layermask;

    RaycastHit hit;
    Shooting shooting;

    void Start()
    {
        SetAmmo();
        shooting = transform.root.GetComponent<Shooting>();
    }

    //public override IEnumerator Fire()
    //{
    //    if (!isFiring)
    //    {
    //        isFiring = true;
    //        shooting.CmdStartMuzzleFlash();
    //        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 1000, layermask))
    //        {
    //            if (hit.transform.tag == "Collision")
    //            {
    //                //CmdPlayerShot(hit.transform.parent.transform.parent.name);
    //            }
    //            else
    //            {
    //                Vector3 position = hit.point + (hit.normal * .1f);
    //                Quaternion rotation = Quaternion.LookRotation(hit.normal);
    //                shooting.CmdBulletHole(position, rotation);
    //            }
    //        }
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
