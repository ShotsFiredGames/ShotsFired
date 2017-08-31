using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Shooting : Photon.MonoBehaviour
{
    public Gun currentGun;
    public GameObject gunHolder;
    GameObject bulletHole;
    GameObject muzzleFlash;
    public LayerMask layermask;
    public Image hitMarker;
    AudioSource hitMarkerSource;
    PlayerManager playerManager;

    [Header("Shooting UI")]
    public Text ammoUI;

    GameObject cam;
    Coroutine overcharged;
    bool isOvercharged;
    short _damage;                // this is the variable that gets affected by overcharged
    GunBob gunBob;
    bool losingAmmo;

    void Start()
    {
        cam = transform.Find("Main Camera").transform.Find("Camera").gameObject;
        playerManager = GetComponent<PlayerManager>();
        hitMarkerSource = hitMarker.GetComponent<AudioSource>();
        ammoUI.text = "";
        gunBob = GetComponentInChildren<GunBob>();
    }

    void Update()
    {
        if (currentGun == null)
        {
            ammoUI.text = "";
            return;
        }
        if (!currentGun.isActiveAndEnabled)
        {
            ammoUI.text = "";
        }
        else
        {
            ammoUI.gameObject.SetActive(true);
            ammoUI.text = currentGun.currentAmmo + " / " + currentGun.ammo;
        }
    }

    public IEnumerator Firing()
    {
        if (currentGun.isActiveAndEnabled && !currentGun.isFiring)
        {
            playerManager.FireAnimation();
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
                    photonView.RPC("RPC_SustainedShot", PhotonTargets.Others);
                    break;
                case Gun.WeaponType.Particle:
                    ParticleShot();
                    break;
                default:
                    break;
            }

           if (!isOvercharged)
                currentGun.currentAmmo--;

            gunBob.ApplyRecoil();

            //ammoUI.text = currentGun.currentAmmo + " / " + currentGun.ammo;

            yield return new WaitForSeconds(currentGun.fireFreq);

            if (currentGun.currentAmmo <= 0)
                currentGun.Discard();

            currentGun.isFiring = false;

            yield return new WaitForSeconds(.5f);
            if (currentGun.shootingAnim != null)
                if (currentGun.gameObject.activeSelf)
                    currentGun.shootingAnim.ResetTrigger("Fire");
        }
    }

    public void SetWeapon(Gun weapon)
    {
        currentGun = weapon;

        if (currentGun.shootingAnim != null)
            if (currentGun.gameObject.activeSelf)
                currentGun.shootingAnim.ResetTrigger("Fire");

        currentGun.shootingSource.clip = currentGun.shootingSound;
        currentGun.isFiring = false;
        //ammoUI.text = currentGun.ammo + " / " + currentGun.ammo;
        muzzleFlash = currentGun.muzzleFlash;
    }

    public void RemoveWeapon()
    {
        if (currentGun != null)
        {
            currentGun.isFiring = false;
            if (currentGun.shootingAnim != null)
                if (currentGun.gameObject.activeSelf)
                    currentGun.shootingAnim.ResetTrigger("Fire");
            currentGun.SetActiveGun(false);
            //ammoUI.text = "";
        }
    }

    //maybe make RPC
    public void StopFiring()
    {
        currentGun.shootingSource.PlayOneShot(currentGun.trailClip);
        if(currentGun.weaponType == Gun.WeaponType.Sustained)
        {
            StopSustainedShot();
            photonView.RPC("RPC_StopSustainedShot", PhotonTargets.Others);
        }
    }

    public void Local_StartMuzzleFlash()
    {
        if (muzzleFlash == null) return;
        StartCoroutine(MuzzleFlash());                                                                          //Activate the MuzzleFlash
    }

    [PunRPC]
    public void RPC_StartMuzzleFlash()
    {
        if (muzzleFlash == null) return;
        StartCoroutine(MuzzleFlash());                                                                          //Activate the MuzzleFlash
    }

    IEnumerator MuzzleFlash()                                                                                   //Activate and DeActivate the muzzle flash
    {
        if (currentGun.shootingSource.clip != null)
            currentGun.shootingSource.PlayOneShot(currentGun.shootingSound);
        if (muzzleFlash == null) yield break;

        muzzleFlash.SetActive(true);
        if (currentGun.thirdPersonMuzzle != null)
            currentGun.thirdPersonMuzzle.SetActive(true);
        yield return new WaitForSeconds(currentGun.muzzleFlashTimer);
        if (currentGun.thirdPersonMuzzle != null)
            currentGun.thirdPersonMuzzle.SetActive(false);
        if(muzzleFlash != null)
        muzzleFlash.SetActive(false);
    }

    IEnumerator HitMarker()
    {
        hitMarker.enabled = (true);
        hitMarkerSource.PlayOneShot(hitMarkerSource.clip);
        yield return new WaitForSeconds(0.05f);
        hitMarker.enabled = (false);
    }

    public void SpawnBulletHole(Vector3 position, Quaternion rotation, string hitType)
    {
        switch (hitType)
        {
            case "Wall":
                PhotonNetwork.Instantiate(currentGun.wallHit.name, position, rotation, 0);
                break;
            case "Player":
                PhotonNetwork.Instantiate(currentGun.playerHit.name, position, rotation, 0);
                break;
        }
    }

    void PlayerShot(string hitPlayer, string hitCollider, short _damage)
    {
        PlayerWrangler.GetPlayer(hitPlayer).transform.Find("CollisionDetection").transform.Find(hitCollider).GetComponent<CollisionDetection>().OnHit(_damage, transform.name);
    }

    void ReaperShot(string thisPlayer, string chasingPlayer, short _damage)
    {
        TheReaperComes.GetReaperChasingWhom(chasingPlayer).HitBy(_damage, thisPlayer);
    }

    public RaycastHit CastMyRay()
    {
        RaycastHit hit;
        Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 5000, layermask);
        return hit;
    }

    void HitscanShot()
    {
        Local_StartMuzzleFlash();
        photonView.RPC("RPC_StartMuzzleFlash", PhotonTargets.Others);
        RaycastHit hit = CastMyRay();
        if (hit.point == Vector3.zero) return;

        if (hit.transform.tag.Equals("Collision"))
        {
            StartCoroutine(HitMarker());
            PlayerShot(hit.transform.root.name, hit.transform.name, _damage);
            Vector3 position = hit.point + (hit.normal * .1f);
            Quaternion rotation = Quaternion.LookRotation(hit.normal);

            if (PhotonNetwork.isMasterClient)
                SpawnBulletHole(position, rotation, "Player");
        }
        else if (hit.transform.tag.Equals("Reaper"))
        {
            StartCoroutine(HitMarker());
            ReaperShot(transform.root.name, hit.transform.GetComponent<Reaper>().GetTargetPlayer(), _damage);
        }
        else
        {
            Vector3 position = hit.point + (hit.normal * .1f);
            Quaternion rotation = Quaternion.LookRotation(hit.normal);
            SpawnBulletHole(position, rotation, "Wall");
        }

        Vector3 direction = hit.point;
        Vector3 hitNormal = hit.normal;
        GameObject bullet = PhotonNetwork.Instantiate(currentGun.projectile.name, currentGun.gunbarrel.transform.position, currentGun.gunbarrel.transform.rotation, 0);
        bullet.GetComponent<Projectile>().Local_SetProjectileVariables(currentGun.speed, direction, transform.name, hitNormal, 0, false);
        bullet.GetComponent<PhotonView>().RPC("RPC_SetProjectileVariables", PhotonTargets.Others, currentGun.speed, direction, transform.name, hitNormal, (short)0, false);
    }

    void ParticleShot()
    {
        Local_StartMuzzleFlash();
        photonView.RPC("RPC_StartMuzzleFlash", PhotonTargets.Others);
    }

    void ProjectileShot()
    {
        Local_StartMuzzleFlash();
        photonView.RPC("RPC_StartMuzzleFlash", PhotonTargets.Others);
        RaycastHit hit = CastMyRay();
        ProjectileShot(hit.point, hit.normal);
    }

    void ProjectileShot(Vector3 direction, Vector3 hitNormal)
    {
        if (currentGun != null)
        {
            if (currentGun.shootingAnim != null)
                if (currentGun.gameObject.activeSelf)
                    currentGun.shootingAnim.SetTrigger("Fire");

            GameObject bullet = PhotonNetwork.Instantiate(currentGun.projectile.name, currentGun.gunbarrel.transform.position, currentGun.gunbarrel.transform.rotation, 0);

            if(currentGun.gunName == "MineGun")
            {
                bullet.GetComponent<Mine>().Local_SetProjectileVariables(currentGun.speed, direction, transform.name, hitNormal, _damage, true);
                bullet.GetComponent<PhotonView>().RPC("RPC_SetProjectileVariables", PhotonTargets.Others, currentGun.speed, direction, transform.name, hitNormal, _damage, true);
            }
            else
            {
                bullet.GetComponent<Projectile>().Local_SetProjectileVariables(currentGun.speed, direction, transform.name, hitNormal, _damage, true);
                bullet.GetComponent<PhotonView>().RPC("RPC_SetProjectileVariables", PhotonTargets.Others, currentGun.speed, direction, transform.name, hitNormal, _damage, true);
            }
        }
    }

    void SustainedShot()
    {
        currentGun.sustainedCollider.enabled = false;
        currentGun.sustainedCollider.enabled = true;
        currentGun.sustainedEffect.SetActive(true);
    }

    void StopSustainedShot()
    {
        currentGun.sustainedCollider.enabled = false;
        currentGun.sustainedEffect.SetActive(false);
    }

    [PunRPC]
    void RPC_SustainedShot()
    {
        if (currentGun.thirdPersonSustained != null)
            currentGun.thirdPersonSustained.SetActive(true);

        currentGun.sustainedEffect.SetActive(true);
    }

    [PunRPC]
    void RPC_StopSustainedShot()
    {
        if(currentGun.thirdPersonSustained != null)
            currentGun.thirdPersonSustained.SetActive(false);

        currentGun.sustainedEffect.SetActive(false);
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

    public void Aiming()
    {
        if (currentGun == null) return;
        currentGun.unArmedReticle.SetActive(false);
        currentGun.armedReticle.SetActive(false);
        currentGun.scopedReticle.SetActive(true);
    }

    public void NotAiming()
    {
        if (currentGun == null) return;
        currentGun.unArmedReticle.SetActive(false);
        currentGun.scopedReticle.SetActive(false);
        currentGun.armedReticle.SetActive(true);
    }

    public void UnArmed()
    {
        if (currentGun == null) return;
        currentGun.scopedReticle.SetActive(false);
        currentGun.armedReticle.SetActive(false);
        currentGun.unArmedReticle.SetActive(true);
    }
}