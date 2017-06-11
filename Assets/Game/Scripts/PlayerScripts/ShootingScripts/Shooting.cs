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
            StartCoroutine(LocalMuzzleFlash());
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
        StartCoroutine(ThirdMuzzleFlash());                                                                  //Activate the MuzzleFlash
    }

    IEnumerator ThirdMuzzleFlash()                                                                                   //Activate and DeActivate the muzzle flash
    {
        if (currentGun.thirdPersonMuzzle == null) currentGun.FindThirdPersonInfo();
        currentGun.thirdPersonMuzzle.SetActive(true);
        yield return new WaitForSeconds(.50f);
        currentGun.thirdPersonMuzzle.SetActive(false);
    }

    IEnumerator LocalMuzzleFlash()                                                                                   //Activate and DeActivate the muzzle flash
    {
        _muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(.5f);
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
        if (isLocalPlayer) return;


        //Projectile bullet = currentGun.projectile.GetPooledInstance<Projectile>();
        if (objectPooling == null)
        {
            objectPooling = GameObject.Find("GameManager").GetComponent<NetworkedPoolingScript>();
            Debug.LogError("object pooling was null");
        }

        GameObject bullet = objectPooling.GetFromPool(position);

        if (bullet == null)
        {
            Debug.LogError("There is no bullet");
            return;
        }

        bullet.transform.position = position;
        bullet.transform.rotation = rotation;
        bullet.GetComponent<Projectile>().SetVariables(speed, direction, false);

        Debug.LogError("Shoot Online " + bullet);
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

        CmdSpawnProjectile(currentGun.thirdPersonGunBarrel.transform.position, transform.root.rotation, hit.point, currentGun.speed);

        //Shoot Different Projectile Locally//
        if (objectPooling == null)
        {
            objectPooling = GameObject.Find("GameManager").GetComponent<NetworkedPoolingScript>();
            Debug.LogError("object pooling was null");
        }

        Vector3 position = currentGun.gunbarrel.transform.position;
        Debug.LogError("Position is: " + position);

        GameObject bullet = objectPooling.GetFromPool(position);

        if (bullet == null)
        {
            Debug.LogError("There is no bullet");
            return;
        }

        //bullet.transform.position = position;
        //bullet.GetComponent<Rigidbody>().velocity = hit.point * (float)currentGun.speed;

        bullet.transform.rotation = transform.root.rotation;
        bullet.GetComponent<Projectile>().SetVariables(currentGun.speed, hit.point, true);

        Debug.LogError("Shoot Locally " + bullet);
    }

    [Client]
    void SustainedShot()
    {

    }
}
