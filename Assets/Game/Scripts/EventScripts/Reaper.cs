using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class Reaper : NetworkBehaviour
{
    [Header("Stats")]
    public float speed;
    public float respawnTime;
    public short health;
    public bool canDie = true;
    [Header("Player Interaction")]
    public float distanceToAttack;
    public float shotSpeedIncrease;
    [Tooltip("The vertical offset for the players. 0 means feet.")]
    public float attackingOffset;
    

    [SyncVar]
    short currentHealth;
    short points;
    bool isDead;

    [SyncVar]
    private Vector3 spawnPoint;
    private float currentDistance;
    [SyncVar]
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
        transform.position = spawnPoint;
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
                Vector3 playerPosition = new Vector3(targetPlayer.transform.position.x, targetPlayer.transform.position.y + attackingOffset, targetPlayer.transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, playerPosition, step);
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
            RpcReaperTookDamage(damage);
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
        Debug.LogError("Shot Reaper. Health is: " + currentHealth);

        if (currentHealth <= 0 && !isDead)
            StartCoroutine(Respawn());

    }

    public string GetTargetPlayer()
    {
        if (targetPlayer == null) return "No one";
        return targetPlayer.transform.name;
    }

    public void SetTargetPlayer(PlayerManager target)
    {
        targetPlayer = target;
        targetPlayer.ReaperEffectsActivate(true);
    }

    public void SetSpawnPoint(Transform spawn)
    {
        Debug.LogError("Spawn point: " + spawn + " set for the reaper chasing " + GetTargetPlayer());
        spawnPoint = spawn.position;
    }

    public void SetPoints(short points)
    {
        this.points = points;
    }

    IEnumerator Respawn()
    {
        CmdSetReaperPosition(new Vector3(-1000, -1000, -1000), GetTargetPlayer());
        isDead = true;
        Debug.LogError("Current Position is: " + transform.root.position);
        yield return new WaitForSeconds(1f);
        Debug.LogError("Spawn point: " + spawnPoint);
        CmdSetReaperPosition(spawnPoint, GetTargetPlayer());
        transform.rotation = Quaternion.identity;
        currentSpeed = speed;

        currentHealth = health;
        isDead = false;
        gameObject.SetActive(true);
    }

    [Command]
    void CmdSetReaperPosition(Vector3 newPosition, string whichReaper)
    {
        Debug.LogError("In the command. New Position is: " + newPosition);
        RpcSetReaperPosition(newPosition, whichReaper);
    }

    [ClientRpc]
    void RpcSetReaperPosition(Vector3 newPosition, string whichReaper)
    {
        Debug.LogError("In the rpc. New position is: " + newPosition);
        TheReaperComes.GetReaperChasingWhom(whichReaper).transform.position = newPosition;
    }

    public void StopReaper()
    {
        if (respawn != null)
            StopCoroutine(respawn);

        gameObject.SetActive(false);
        this.enabled = false;
    }
}
