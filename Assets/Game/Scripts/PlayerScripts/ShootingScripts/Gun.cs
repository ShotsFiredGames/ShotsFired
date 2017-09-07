using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum WeaponType
    {
        Hitscan,
        Projectile,
        Sustained,
        Particle
    };

    [Header("Gun Animation Variables")]
    public float recoilAmount;
    public float recoilRecoverTime;
    [HideInInspector]
    public float currentRecoilZPositon;
    [HideInInspector]
    public float currentRecoilZPositionVelocity;

    public bool canAim;
    public WeaponType weaponType;
	public AudioSource shootingSource;

    public GameObject thirdPersonGun;
    public string gunName;
    public short damage;
    public byte ammo;
    public float fireFreq;
    public Transform spawnPoint;
    public Vector3 basePosition;
	public Vector3 baseRotation;
    public Vector3 aimPosition;
	public Vector3 aimRotation;
    public AudioClip shootingSound;
    public AudioClip trailClip;
    public GameObject muzzleFlash;
    public Animator anim;
    public GameObject playerHit;
    public GameObject wallHit;
	public float muzzleFlashTimer;

    public PlayerManager playerManager;

    public GameObject sustainedEffect;
    public GameObject thirdPersonSustained;
    public Collider sustainedCollider;

    [HideInInspector]
    public bool isFiring = false;

    [HideInInspector]
    public bool isAmmoUnlimited;
    [HideInInspector]
    public byte currentAmmo;

    public GameObject projectile;
    public float speed;
    public GameObject gunbarrel;
    public GameObject thirdPersonGunBarrel;
    public GameObject thirdPersonMuzzle;

    public Animator shootingAnim;

    public GameObject unArmedReticle;
    public GameObject armedReticle;
    public GameObject scopedReticle;

    void Start()
    {
        SetAmmo();
    }

    public void Discard()
    {
        playerManager.Local_Disarm();
        playerManager.PhotonView.RPC("RPC_Disarm", PhotonTargets.Others);
    }

    public void SetAmmo()
    {
        this.isAmmoUnlimited = GameCustomization.isAmmoUnlimited;
        currentAmmo = ammo;
    }

    public void UseAmmo()
    {
        if (isAmmoUnlimited) return;

        currentAmmo--;
        if (currentAmmo <= 0)
            Discard();
    }

    public void SetActiveGun(bool isActive)
    {
        if (playerManager.isActiveAndEnabled)
            gameObject.SetActive(isActive);
        else
            thirdPersonGun.SetActive(isActive); 
    }
}
