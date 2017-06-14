using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerHealth : NetworkBehaviour
{
    PlayerManager playerManager;

    public Image dmgIndicatorLeft;
    public Image dmgIndicatorRight;
    public Image dmgIndicatorTop;
    public Image dmgIndicatorBottom;

    public AudioSource source;
    public AudioClip[] hitEffects;
    float respawnTime;
    float currMaxHealth;
    float maxHealth;
    [SyncVar]
    float currentHealth;

    [SyncVar]
    bool isDead;

    bool isHealthIncreased;

    GameObject collisionDetection;

    // Use this for initialization
    void Awake()
    {
        respawnTime = GameCustomization.respawnTime;
        maxHealth = GameCustomization.playerHealth;
        playerManager = GetComponent<PlayerManager>();
    }

    public void Init()
    {
        isDead = false;
        currentHealth = maxHealth;
        currMaxHealth = maxHealth;
        if (!isLocalPlayer)
        {
            collisionDetection = transform.Find("CollisionDetection").gameObject;
            foreach (Transform go in collisionDetection.GetComponentsInChildren<Transform>())
                go.gameObject.layer = LayerMask.NameToLayer("Collision");
        }
    }

    [Command]
    public void CmdTookDamage(int damage, CollisionDetection.CollisionFlag collisionLocation)
    {
        RpcTookDamage(damage, collisionLocation);
    }

    [ClientRpc]
    public void RpcTookDamage(int damage, CollisionDetection.CollisionFlag collisionLocation)                //This is called from CollisionDetection to determine the damage and the location of the incoming collision.
    {
        currentHealth -= damage;
        Hit(collisionLocation);
        if (!source.isPlaying)
        {
            source.clip = hitEffects[Random.Range(0, hitEffects.Length)];
            source.Play();
        }

        if (currentHealth <= 0)
        {
            Died(collisionLocation);
        }
    }

    [Command]
    public void CmdHeadshotDamage(int headshotDamage, CollisionDetection.CollisionFlag collisionLocation)
    {
        RpcHeadshotDamage(headshotDamage, collisionLocation);
    }

    [ClientRpc]
    public void RpcHeadshotDamage(int headshotDamage, CollisionDetection.CollisionFlag collisionLocation)    //This is called from CollisionDetection to determine the damage and the location of the incoming collision.
    {
        if (!source.isPlaying)
        {
            source.clip = hitEffects[Random.Range(0, hitEffects.Length)];
            source.Play();
        }
        Hit(collisionLocation);
        currentHealth -= headshotDamage;

        if (currentHealth <= 0)
        {
            Died(collisionLocation);
        }
    }

    [Command]
    public void CmdInstantDeath(CollisionDetection.CollisionFlag collisionLocation)
    {
        RpcInstantDeath(collisionLocation);
    }

    [ClientRpc]
    public void RpcInstantDeath(CollisionDetection.CollisionFlag collisionLocation)
    {
        Debug.LogError("Dead");
        currentHealth = 0;
        Died(collisionLocation);
    }

    void Died(CollisionDetection.CollisionFlag collisionLocation)                                           //Died gets called when health is or goes below 0.
    {
        isDead = true;

        //foreach (Transform go in collisionDetection.GetComponentsInChildren<Transform>())
        //    go.gameObject.layer = LayerMask.NameToLayer("Default");

        playerManager.Dead(collisionLocation);
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTime);
        Transform respawnpoint = NetworkManager.singleton.GetStartPosition();
        transform.position = respawnpoint.position;
        transform.rotation = respawnpoint.rotation;

        Init();
        playerManager.Respawn();
    }

    [Client]
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
        yield return new WaitForSeconds(0.05f);
        indicator.enabled = false;
    }

    public bool isPlayerDead()
    {
        return (currentHealth <= 0);
    }

    public IEnumerator IncreaseMaxHealth()
    {
        if (!isHealthIncreased)
        {
            isHealthIncreased = true;
            currMaxHealth = maxHealth * 4;
            currentHealth = currMaxHealth;
            yield return new WaitForSeconds(GameCustomization.abilityDuration);
            currMaxHealth = maxHealth;
            if (currentHealth > maxHealth)
                currentHealth = maxHealth;

            isHealthIncreased = false;
        }
        
    }
}