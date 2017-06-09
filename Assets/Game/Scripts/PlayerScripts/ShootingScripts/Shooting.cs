using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Shooting : NetworkBehaviour
{
    public Gun currentGun;
    GameObject _bulletHole;
    GameObject _muzzleFlash;
    public LayerMask layermask;

    GameObject cam;
    NetworkedPoolingScript objectPooling;

    void Start()
    {
        cam = transform.Find("Main Camera").gameObject;
        objectPooling = GameObject.Find("GameManager").GetComponent<NetworkedPoolingScript>();
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
        _bulletHole = currentGun.bulletHole;
        _muzzleFlash = currentGun.muzzleFlash;
    }

    public void RemoveWeapon()
    {
        currentGun.SetActiveGun(false);
        //currentGun = null;
    }

    [Command]
    public void CmdStartMuzzleFlash()
    {
        if (_bulletHole == null) return;
        RpcStartMuzzleFlash();
    }

    [ClientRpc]
    void RpcStartMuzzleFlash()
    {
        StartCoroutine(MuzzleFlash());                                                                  //Activate the MuzzleFlash
    }

    IEnumerator MuzzleFlash()                                                                                   //Activate and DeActivate the muzzle flash
    {
        _muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(.05f);
        _muzzleFlash.SetActive(false);
    }

    [Command]
    public void CmdBulletHole(Vector3 position, Quaternion rotation)
    {
        GameObject hole = Instantiate(_bulletHole, position, rotation) as GameObject;
        hole.SetActive(true);
        NetworkServer.Spawn(hole);
    }

    [Command]
    public void CmdSpawnProjectile(Vector3 position, Quaternion rotation, Vector3 direction, double speed)
    {
        Debug.LogError("Fire Gun Command");
        RpcSpawnProjectile(position, rotation, direction, speed);
    }

    [ClientRpc]
    public void RpcSpawnProjectile(Vector3 position, Quaternion rotation, Vector3 direction, double speed)
    {
        Debug.LogError("Fire Gun RPC");
        //Projectile bullet = currentGun.projectile.GetPooledInstance<Projectile>();
        if (objectPooling == null) objectPooling = GameObject.Find("GameManager").GetComponent<NetworkedPoolingScript>();

        GameObject bullet = objectPooling.GetFromPool(position);

        if (bullet == null)
        {
            return;
        }
        bullet.transform.position = position;
        bullet.transform.rotation = rotation;
        bullet.GetComponent<Projectile>().SetVariables(speed, direction);
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
        if (hit.point == Vector3.zero) return;
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
        CmdStartMuzzleFlash();
        RaycastHit hit = CastMyRay();
        //if (hit.point == Vector3.zero) return;

        CmdSpawnProjectile(currentGun.gunbarrel.transform.position, transform.root.rotation, hit.point, currentGun.speed);
    }

    [Client]
    void SustainedShot()
    {

    }
}
