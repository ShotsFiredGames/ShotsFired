using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Shooting : NetworkBehaviour
{
    public Gun currentGun;
    public GameObject bulletHole;
    public GameObject muzzleFlash;
    public LayerMask layermask;

    GameObject cam;

    void Start()
    {
        cam = transform.Find("Main Camera").gameObject;
    }

    [Client]
    public IEnumerator Firing()
    {
        if (currentGun.isActiveAndEnabled && !currentGun.isFiring)
        {
            currentGun.isFiring = true;
            switch (currentGun.weaponType)
            {
                case Gun.WeaponType.Hitscan:
                    HitscanShot();
                    break;
                case Gun.WeaponType.Projectile:
                    ProjectileShot();
                    break;
                case Gun.WeaponType.Sustained:
                    SustainedShot();
                    break;
                default:
                    break;
            }
            
            yield return new WaitForSeconds(currentGun.fireFreq);
            Debug.LogError(currentGun);
            currentGun.isFiring = false;
            currentGun.UseAmmo();
        }
    }

    public void SetWeapon(Gun weapon)
    {
        currentGun = weapon;
    }

    public void RemoveWeapon()
    {
        currentGun.SetActiveGun(false);
        //currentGun = null;
    }

    [Command]
    public void CmdStartMuzzleFlash()
    {
        if (muzzleFlash == null) return;
        RpcStartMuzzleFlash();
    }

    [ClientRpc]
    void RpcStartMuzzleFlash()
    {
        StartCoroutine(MuzzleFlash());                                                                  //Activate the MuzzleFlash
    }

    IEnumerator MuzzleFlash()                                                                                   //Activate and DeActivate the muzzle flash
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(.05f);
        muzzleFlash.SetActive(false);
    }

    [Command]
    public void CmdBulletHole(Vector3 position, Quaternion rotation)
    {
        GameObject hole = Instantiate(bulletHole, position, rotation) as GameObject;
        NetworkServer.Spawn(hole);
    }

    [Command]
    void CmdPlayerShot(string hitPlayer, string hitCollider)
    {
        PlayerWrangler.GetPlayer(hitPlayer).transform.Find("CollisionDetection").transform.Find(hitCollider).GetComponent<CollisionDetection>().OnHit(currentGun.damage);
        //hitPlayer.OnHit(damage);
    }

    RaycastHit CastMyRay()
    {
        RaycastHit hit;
        Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 1000, layermask);
        return hit;
    }

    [Client]
    void HitscanShot()
    {        
        CmdStartMuzzleFlash();
        RaycastHit hit = CastMyRay();
        if (hit.transform.tag.Equals("Collision"))
        {
            CmdPlayerShot(hit.transform.root.name, hit.transform.name);
        }
        else
        {
            Vector3 position = hit.point + (hit.normal * .1f);
            Quaternion rotation = Quaternion.LookRotation(hit.normal);
            CmdBulletHole(position, rotation);
        }        
    }

    [Client]
    void ProjectileShot()
    {

    }

    [Client]
    void SustainedShot()
    {

    }
}
