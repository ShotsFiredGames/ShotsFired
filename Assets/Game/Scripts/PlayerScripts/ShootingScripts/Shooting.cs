using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Shooting : NetworkBehaviour
{
    public Gun currentGun;
    public GameObject gunHolder;
    public GameObject bulletHole;
    GameObject muzzleFlash;
    public LayerMask layermask;
    public Image hitMarker;
    public AudioSource shootingSource;

    GameObject cam;
    Coroutine overcharged;
    bool isOvercharged;
    byte _damage;                // this is the variable that gets affected by overcharged

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

            if (isOvercharged)
                _damage = (byte)(currentGun.damage * 2);
            else
                _damage = currentGun.damage;

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

            if (!isOvercharged)
                currentGun.UseAmmo();

            currentGun.isFiring = false;
        }
    }

    public void SetWeapon(Gun weapon)
    {
        currentGun = weapon;
        shootingSource.clip = currentGun.shootingSound;
        currentGun.isFiring = false;
        muzzleFlash = currentGun.muzzleFlash;
    }

    public void RemoveWeapon()
    {
        if(currentGun != null)
        {
            currentGun.isFiring = false;
            currentGun.SetActiveGun(false);
        }
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

    [Command]
    void CmdPlayerShot(string hitPlayer, string hitCollider, byte _damage)
    {
        PlayerWrangler.GetPlayer(hitPlayer).transform.Find("CollisionDetection").transform.Find(hitCollider).GetComponent<CollisionDetection>().OnHit(_damage, transform.name);
    }

    public RaycastHit CastMyRay()
    {
        RaycastHit hit;
        Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 5000, layermask);
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
            CmdPlayerShot(hit.transform.root.name, hit.transform.name, _damage);
        }
        else if (hit.transform.tag.Equals("Reaper"))
        {
            StartCoroutine(HitMarker());
            hit.transform.GetComponent<Reaper>().HitBy(_damage, transform.root.name);
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
    void CmdProjectileShot(Vector3 direction, Vector3 hitNormal)
    {
        if (currentGun != null)
        {
            if (currentGun.shootingAnim != null)
                currentGun.shootingAnim.SetTrigger("Fire");
            GameObject bullet = Instantiate(currentGun.projectile, currentGun.gunbarrel.transform.position, currentGun.gunbarrel.transform.rotation) as GameObject;
            NetworkServer.Spawn(bullet);
            RpcProjectileShot(bullet.GetComponent<NetworkIdentity>(), direction, hitNormal);
        }
    }

    [ClientRpc]
    void RpcProjectileShot(NetworkIdentity bullet, Vector3 direction, Vector3 hitNormal)
    {
        if (bullet.gameObject != null)
            bullet.GetComponent<Projectile>().SetVariables(currentGun.speed, direction, transform.name, hitNormal, _damage);
    }

    [Client]
    void SustainedShot()
    {

    }

    public void ActivateOvercharged()
    {
        overcharged = StartCoroutine(OverchargedAbility());
    }

    IEnumerator OverchargedAbility()
    {
        isOvercharged = true;
        yield return new WaitForSeconds(GameCustomization.abilityDuration);
        isOvercharged = false;
    }

    public void CancelOvercharged()
    {
        if (overcharged != null)
        {
            StopCoroutine(overcharged);
            isOvercharged = false;
        }
    }
}
