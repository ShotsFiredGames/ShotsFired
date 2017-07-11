﻿using System.Collections;
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
    public Material dmgEffect;
    public GameObject despawnEffect;

    public Animator damageEffectAnim;
    public AudioSource source;
    public AudioClip[] hitEffects;
    float respawnTime;
    short currMaxHealth;
    short maxHealth;
    [SyncVar]
    short currentHealth;

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
    public void CmdTookDamage(byte damage, string sourceID, CollisionDetection.CollisionFlag collisionLocation)
    {
        if (isDead) return;
        RpcTookDamage(damage, sourceID, collisionLocation);
    }

    [ClientRpc]
    public void RpcTookDamage(byte damage, string sourceID, CollisionDetection.CollisionFlag collisionLocation)                //This is called from CollisionDetection to determine the damage and the location of the incoming collision.
    {
        if (isDead) return;
        currentHealth -= damage;
        Hit(collisionLocation);
        if (!source.isPlaying)
        {
            source.clip = hitEffects[Random.Range(0, hitEffects.Length)];
            source.Play();
        }

        if(currentHealth > (short)(maxHealth * 0.75f))
        {
            StopHeartbeat();
        }
        else if(currentHealth <= (short)(maxHealth  * 0.75f) && currentHealth > (short)(maxHealth * 0.5f))
        {
            damageEffectAnim.SetInteger("DamageEffect", 1);
        }
        else if(currentHealth <= (short)(maxHealth * 0.5f) && currentHealth > (short)(maxHealth * 0.25f))
        {
            damageEffectAnim.SetInteger("DamageEffect", 2);
        }
        else if(currentHealth <= (short)(maxHealth * 0.25f) && currentHealth != 0)
        {
            damageEffectAnim.SetInteger("DamageEffect", 3);
        }
        else if(currentHealth <= 0)
        {
            Died(sourceID, collisionLocation);
        }
    }

    void StopHeartbeat()
    {
        damageEffectAnim.SetInteger("DamageEffect", 0);
    }


    [Command]
    public void CmdInstantDeath(string damageSource, CollisionDetection.CollisionFlag collisionLocation)
    {
        if (isDead) return;
        RpcInstantDeath(damageSource, collisionLocation);
    }

    [ClientRpc]
    public void RpcInstantDeath(string damageSource, CollisionDetection.CollisionFlag collisionLocation)
    {
        if (isDead) return;
        currentHealth = 0;
        Died(damageSource, collisionLocation);
    }

    [Client]
    void Died(string damageSource, CollisionDetection.CollisionFlag collisionLocation)                                           //Died gets called when health is or goes below 0.
    {
        StopHeartbeat();
        isDead = true;
        //foreach (Transform go in collisionDetection.GetComponentsInChildren<Transform>())
        //    go.gameObject.layer = LayerMask.NameToLayer("Default");

        StartCoroutine(DespawnEffect());
        playerManager.Dead(damageSource, collisionLocation);
        StartCoroutine(Respawn());
    }

    IEnumerator DespawnEffect()
    {
        yield return new WaitForSeconds(1.5f);
        despawnEffect.SetActive(true);
    }

    IEnumerator Respawn()
    {        
        yield return new WaitForSeconds(respawnTime);
        despawnEffect.SetActive(false);
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
        dmgEffect.color = Color.red;
        yield return new WaitForSeconds(0.05f);
        dmgEffect.color = Color.clear;
        indicator.enabled = false;
    }

    public bool isPlayerDead()
    {
        return (currentHealth <= 0);
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