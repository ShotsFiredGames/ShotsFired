using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class Reaper : NetworkBehaviour
{
    [Header("Stats")]
    public float speed;
    public float respawnTime;
    public short health;
    public bool canDie;
    [Header("Player Interaction")]
    public float distanceToAttack;
    public float shotSpeedIncrease;
    

    [SyncVar]
    short currentHealth;
    short points;

    private Transform spawnPoint;
    private float currentDistance;
    private float currentSpeed;
    PlayerManager targetPlayer;
    GameObject targetPlayerObject;
    GameManager gameManager;
    Coroutine respawn;

    public void Setup()
    {
        gameObject.SetActive(true);
        currentSpeed = speed;
        currentHealth = health;
        transform.position = spawnPoint.position;
    }

    void FixedUpdate()
    {
        if (targetPlayer != null)
        {
            currentDistance = Vector3.Distance(targetPlayer.transform.position, this.transform.position);

            currentDistance = Mathf.Abs(currentDistance);

            if (currentDistance > distanceToAttack)
            {
                float step = currentSpeed * Time.deltaTime / 2;
                transform.position = Vector3.MoveTowards(transform.position, targetPlayer.transform.position, step);
            }
            else
            {
                if (!targetPlayer.isDead)
                {
                    targetPlayer.GetComponent<PlayerHealth>().CmdInstantDeath("Reaper", CollisionDetection.CollisionFlag.Back);
                    GameManager.instance.CmdAddScore(GetTargetPlayer(), (short)-points);

                    if (!canDie)
                        StopReaper();
                }
            }
        }
    }

    public void HitBy(short damage, string player)
    {
        if (!player.Equals(GetTargetPlayer()))
            IncreaseSpeed();
        else
        {
            if (canDie)
                CmdReaperTookDamage(damage);
        }
            
    }

    public void IncreaseSpeed()
    {
        currentSpeed += shotSpeedIncrease;
    }

    [Command]
    public void CmdReaperTookDamage(short damage)
    {
        RpcReaperTookDamage(damage);
    }

    [ClientRpc]
    public void RpcReaperTookDamage(short damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
            StartCoroutine(Respawn());

    }

    public string GetTargetPlayer()
    {
        return targetPlayer.transform.name;
    }

    public void SetTargetPlayer(PlayerManager target)
    {
        targetPlayer = target;
        targetPlayer.ReaperEffectsActivate(true);
    }

    public void SetSpawnPoint(Transform spawn)
    {
        spawnPoint = spawn;
    }

    public void SetPoints(short points)
    {
        this.points = points;
    }

    IEnumerator Respawn()
    {
        transform.position = new Vector3(-1000, -1000, -1000);
        yield return new WaitForSeconds(1f);
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
        currentSpeed = speed;

        currentHealth = health;
        gameObject.SetActive(true);
    }

    public void StopReaper()
    {
        if (respawn != null)
            StopCoroutine(respawn);

        gameObject.SetActive(false);
        this.enabled = false;
    }
}
