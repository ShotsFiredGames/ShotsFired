using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Photon.MonoBehaviour
{
    PlayerManager playerManager;

    public PhotonView PhotonView { get; private set; }

    public Image dmgIndicatorLeft;
    public Image dmgIndicatorRight;
    public Image dmgIndicatorTop;
    public Image dmgIndicatorBottom;
    public Material dmgEffect;
    public GameObject despawnEffect;
    public GameObject[] collisionLocations;
    public Animator damageEffectAnim;
    public AudioSource source;
    public AudioClip[] hitEffects;

    public bool isBeingHealed { get; set; }
    public bool isDead { get; set; }

    float respawnTime;
    float restoreHealth;
    double restoreFreq;
    short currMaxHealth;
    short maxHealth;
    short currentHealth;
    
    bool isHealthIncreased;

    GameObject collisionDetection;
    Coroutine respawn;
    BallToTheWall ballToTheWall;
    Transform respawnpoint;

    public SkinnedMeshRenderer[] playerMeshes;

    [Header("Health UI")]
    public Text health;

    void Awake()
    {
        respawnTime = GameCustomization.respawnTime;
        maxHealth = GameCustomization.playerHealth;
        playerManager = GetComponent<PlayerManager>();
        PhotonView = GetComponent<PhotonView>();
        health.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        CinematicCameraSystem.OnCinematicFinished += ActivateUI;
    }

    private void Start()
    {
        ballToTheWall = GameObject.Find("BallToTheWall").GetComponent<BallToTheWall>();
    }

    void ActivateUI()
    {
        if (!PhotonNetwork.player.NickName.Equals(name))
            return;

        health.gameObject.SetActive(true);
    }

    public void Init()
    {
        isDead = false;
        currentHealth = maxHealth;
        currMaxHealth = maxHealth;

        SetDamageEffect();
        SetHealthUI();

        if (!photonView.isMine)
        {
            collisionDetection = transform.Find("CollisionDetection").gameObject;
            foreach (Transform go in collisionDetection.GetComponentsInChildren<Transform>())
                go.gameObject.layer = LayerMask.NameToLayer("Collision");            
        }
    }

    public void Local_TookDamage(short damage, string sourceID, CollisionDetection.CollisionFlag collisionLocation)                //This is called from CollisionDetection to determine the damage and the location of the incoming collision.
    {
        if (!photonView.isMine) return;
        if (isDead) return;
        currentHealth -= damage;
        Hit(collisionLocation);
        if (!source.isPlaying)
        {
            source.clip = hitEffects[Random.Range(0, hitEffects.Length)];
            source.Play();
        }

        SetDamageEffect();
        SetHealthUI();
        if (currentHealth <= 0)
            Died(sourceID, collisionLocation);
    }

    [PunRPC]
    public void RPC_TookDamage(short damage, string sourceID, CollisionDetection.CollisionFlag collisionLocation)                //This is called from CollisionDetection to determine the damage and the location of the incoming collision.
    {
        if (!photonView.isMine) return;
        if (isDead) return;
        currentHealth -= damage;
        if (currentHealth <= 0)
            Died(sourceID, collisionLocation);

        SetHealthUI();
    }

    public void Local_RestoreHealth(byte healthRestore)
    {
        if (!photonView.isMine) return;
        if (isDead) return;
        currentHealth += healthRestore;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        SetDamageEffect();
        SetHealthUI();
    }

    [PunRPC]
    public void RPC_RestoreHealth(byte healthRestore)
    {
        if (!photonView.isMine) return;
        if (isDead) return;
        currentHealth += healthRestore;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        SetHealthUI();
    }

    void SetDamageEffect()
    {
        if (currentHealth > (short)(maxHealth * 0.75f))
        {
            StopHeartbeat();
        }
        else if (currentHealth > (short)(maxHealth * 0.5f))
        {
            damageEffectAnim.SetInteger("DamageEffect", 1);
        }
        else if (currentHealth > (short)(maxHealth * 0.25f))
        {
            damageEffectAnim.SetInteger("DamageEffect", 2);
        }
        else if (currentHealth > 0)
        {
            damageEffectAnim.SetInteger("DamageEffect", 3);
        }        
    }

    void SetHealthUI()
    {
        health.text = currentHealth + " / " + currMaxHealth;
    }

    void StopHeartbeat()
    {
        damageEffectAnim.SetInteger("DamageEffect", 0);
    }

    public void Local_InstantDeath(string damageSource, CollisionDetection.CollisionFlag collisionLocation)
    {
        if (isDead) return;
        currentHealth = 0;
        SetHealthUI();
        Died(damageSource, collisionLocation);
    }

    [PunRPC]
    public void RPC_InstantDeath(string damageSource, CollisionDetection.CollisionFlag collisionLocation)
    {
        if (isDead) return;
        currentHealth = 0;
        SetHealthUI();
        Died(damageSource, collisionLocation);
    }

    void Died(string damageSource, CollisionDetection.CollisionFlag collisionLocation)                                           //Died gets called when health is or goes below 0.
    {
        if (isDead == true) return;
        if (!isDead)
        {
            isDead = true;
            StopHeartbeat();

            foreach (GameObject go in collisionLocations)
                go.layer = LayerMask.NameToLayer("Default");

            StartCoroutine(DespawnEffect());
            playerManager.Dead(damageSource, collisionLocation);

            if (respawn == null)
                respawn = StartCoroutine(Respawn());
        }
    }

    [PunRPC]
    void RPC_PlayerRenderState(bool isEnabled)
    {
        foreach (SkinnedMeshRenderer rend in playerMeshes)
            rend.enabled = isEnabled;
    }

    [PunRPC]
    void RPC_DespawnEffectState(bool isEnabled)
    {
        despawnEffect.SetActive(isEnabled);
    }

    IEnumerator DespawnEffect()
    {
        yield return new WaitForSeconds(.5f);
        despawnEffect.SetActive(true);
        PhotonView.RPC("RPC_DespawnEffectState", PhotonTargets.Others, true);
    }

    IEnumerator Respawn()
    {
        foreach (SkinnedMeshRenderer rend in playerMeshes)
            rend.enabled = false;
        PhotonView.RPC("RPC_PlayerRenderState", PhotonTargets.Others, false);
        yield return new WaitForSeconds(respawnTime);


        if (ballToTheWall == null)
            ballToTheWall = GameObject.Find("BallToTheWall").GetComponent<BallToTheWall>();

        if (!ballToTheWall.ballToTheWallActive)
            respawnpoint = GameManager.instance.GetSpawnPoint();
        else
            respawnpoint = ballToTheWall.spawnpoints[Random.Range(0, ballToTheWall.spawnpoints.Length)].transform;

        transform.position = respawnpoint.position;
        transform.rotation = respawnpoint.rotation;

        if (!photonView.isMine)
            foreach (GameObject go in collisionLocations)
                go.layer = LayerMask.NameToLayer("Collision");


        foreach (SkinnedMeshRenderer rend in playerMeshes)
            rend.enabled = true;
        PhotonView.RPC("RPC_PlayerRenderState", PhotonTargets.Others, true);
        despawnEffect.SetActive(false);
        PhotonView.RPC("RPC_DespawnEffectState", PhotonTargets.Others, false);

        Init();
        playerManager.Respawn();
        respawn = null;
    }

    void Hit(CollisionDetection.CollisionFlag collisionLocation)
    {
        switch (collisionLocation)
        {
            case CollisionDetection.CollisionFlag.FrontHeadShot:
                StartCoroutine(IndicateDamage(dmgIndicatorTop));
                break;
            case CollisionDetection.CollisionFlag.BackHeadShot:
                StartCoroutine(IndicateDamage(dmgIndicatorBottom));
                break;
            case CollisionDetection.CollisionFlag.Front:
                StartCoroutine(IndicateDamage(dmgIndicatorTop));
                break;
            case CollisionDetection.CollisionFlag.Back:
                StartCoroutine(IndicateDamage(dmgIndicatorBottom));
                break;
            case CollisionDetection.CollisionFlag.Left:
                StartCoroutine(IndicateDamage(dmgIndicatorLeft));
                break;
            case CollisionDetection.CollisionFlag.Right:
                StartCoroutine(IndicateDamage(dmgIndicatorRight));
                break;
        }
    }

    IEnumerator IndicateDamage(Image indicator)
    {
        indicator.enabled = true;
        dmgEffect.color = Color.red;
        yield return new WaitForSeconds(0.05f);
        dmgEffect.color = Color.clear;
        indicator.enabled = false;
    }

    public bool isPlayerDead()
    {
        return isDead;
    }

    public void CancelIncreasedHealth()
    {
        if (isHealthIncreased)
        {
            currMaxHealth = maxHealth;
            if (currentHealth > maxHealth)
                currentHealth = maxHealth;

            isHealthIncreased = false;
        }
    }

    public IEnumerator IncreaseMaxHealth()
    {
        if (!isHealthIncreased)
        {
            isHealthIncreased = true;
            currMaxHealth = (short)(maxHealth * 4);
            currentHealth = currMaxHealth;
            yield return new WaitForSeconds(GameCustomization.abilityDuration);
            currMaxHealth = maxHealth;
            if (currentHealth > maxHealth)
                currentHealth = maxHealth;

            isHealthIncreased = false;
        }
    }
}