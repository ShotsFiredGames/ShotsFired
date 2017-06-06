using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Shooting : NetworkBehaviour
{
    public GameObject currentGun;
    public GameObject bulletHole;
    public GameObject muzzleFlash;
    public Projectile[] projectiles;

    public void Firing()
    {
        print("IsFirin' N' Stuff");
        if (currentGun != null)
            StartCoroutine(currentGun.GetComponent<Gun>().Fire());
    }

    public void SetWeapon(GameObject weapon)
    {
        currentGun = weapon;
    }

    public void RemoveWeapon()
    {
        currentGun.GetComponent<Gun>().SetActiveGun(false);
        currentGun = null;
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
        print("muzzle should flash");
        StartCoroutine(MuzzleFlash());                                                                  //Activate the MuzzleFlash
    }

    IEnumerator MuzzleFlash()                                                                                   //Activate and DeActivate the muzzle flash
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(.05f);
        muzzleFlash.SetActive(false);
    }

    [ClientRpc]
    public void RpcPrint()
    {

        print("Spawn bullet hole");
    }

    [Command]
    public void CmdBulletHole(Vector3 position, Quaternion rotation)
    {
        RpcPrint();
        GameObject hole = Instantiate(bulletHole, position, rotation) as GameObject;
        NetworkServer.Spawn(hole);
    }

    [Command]
    public void CmdSpawnProjectile(Vector3 position, Quaternion rotation, Vector3 direction, double speed, int projectileNum)
    {
        Debug.LogError("Fire Gun");
        Projectile bullet = projectiles[projectileNum].GetPooledInstance<Projectile>();

        if (bullet == null)
        {
            return;
        }
        bullet.transform.position = position;
        bullet.transform.rotation = rotation;

        bullet.SetSpeed(speed);
    }
}
