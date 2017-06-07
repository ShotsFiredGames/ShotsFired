using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HitscanGun : Gun
{
    public LayerMask layermask;
    GameObject myCamera;
    RaycastHit hit;

    void Start()
    {
        SetAmmo();
        //NetworkServer.Spawn(this.gameObject);
        shooting = transform.root.GetComponent<Shooting>();
        myCamera = transform.root.Find("Main Camera").gameObject;
        print("Shooting: " + currentAmmo);
    }
    public override IEnumerator Fire()
    {
        if (!isFiring)
        {
            isFiring = true;

            shooting.CmdStartMuzzleFlash();

            if (Physics.Raycast(myCamera.transform.position, myCamera.transform.forward, out hit, 1000, layermask))
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
                    shooting.CmdBulletHole(position, rotation);

                    shooting.RpcPrint();
                }
            }

            UseAmmo();

            yield return new WaitForSeconds(fireFreq);
            isFiring = false;
        }
    }

    public override void Discard()
    {
        print("discared");
        playerManager.CmdDisarm();
    }
}
