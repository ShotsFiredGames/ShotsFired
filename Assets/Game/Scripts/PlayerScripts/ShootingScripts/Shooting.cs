using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Shooting : NetworkBehaviour
{
    public Gun currentGun;
    public GameObject bulletHole;
    public GameObject muzzleFlash;
    public LayerMask layermask;
    public Image hitMarker;
    public AudioSource shootingSource;

    GameObject cam;

    void Start()
    {
        cam = transform.Find("Main Camera").transform.Find("Camera").gameObject;
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
            currentGun.isFiring = false;
            currentGun.UseAmmo();
        }
    }

    public void SetWeapon(Gun weapon)
    {
        currentGun = weapon;
        shootingSource.clip = currentGun.shootingSound;
    }

    public void RemoveWeapon()
    {
        if(currentGun != null)
        currentGun.SetActiveGun(false);
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
        if (shootingSource.clip != null)
            shootingSource.Play();
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(.05f);
        muzzleFlash.SetActive(false);
    }

    IEnumerator HitMarker()
    {
        hitMarker.enabled = true;
        yield return new WaitForSeconds(0.05f);
        hitMarker.enabled = false;
    }

    [Command]
    public void CmdBulletHole(Vector3 position, Quaternion rotation)
    {
        GameObject hole = Instantiate(bulletHole, position, rotation) as GameObject;
        NetworkServer.Spawn(hole);
    }

    public RaycastHit CastMyRay()
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
        if (hit.point == Vector3.zero) return;
        if (hit.transform.tag.Equals("Collision"))
        {
            StartCoroutine(HitMarker());
            hit.transform.GetComponent<CollisionDetection>().OnHit(currentGun.damage, transform.name);
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
        CmdStartMuzzleFlash();
        RaycastHit hit = CastMyRay();
        CmdProjectileShot(hit.point, hit.normal);
    }

    [Command]
    void CmdProjectileShot(Vector3 direction,Vector3 hitNormal)
    {
        RpcProjectileShot(direction, hitNormal);
    }

    [ClientRpc]
    void RpcProjectileShot(Vector3 direction, Vector3 hitNormal)
    {
        GameObject bullet = Instantiate(currentGun.projectile, currentGun.gunbarrel.transform.position, currentGun.gunbarrel.transform.rotation) as GameObject;
        bullet.GetComponent<Projectile>().SetVariables(currentGun.speed, direction, transform.name, hitNormal, currentGun.damage);
        SpawnObject(bullet);
    }

    [ServerCallback]
    void SpawnObject(GameObject spawningObject)
    {
        NetworkServer.Spawn(spawningObject);
    }

    [Client]
    void SustainedShot()
    {

    }
}
