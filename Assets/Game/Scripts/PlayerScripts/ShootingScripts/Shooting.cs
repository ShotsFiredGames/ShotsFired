using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Shooting : NetworkBehaviour
{
    public GameObject currentGun;
    public GameObject bulletHole;
    public GameObject muzzleFlash;

    public void Firing()
    {
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
}
