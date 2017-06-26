using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Reaper : NetworkBehaviour
{
    [Header("Stats")]
    public float speed;
    public float respawnTime;
    public int health;
    [Header("Player Interaction")]
    public float distanceToAttack;
    public double shotSpeedIncrease;
    

    [SyncVar]
    int currentHealth;
    int points;

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

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    void FixedUpdate()
    {
        targetPlayerObject = GameObject.Find(gameManager.GetWinningPlayer());

        if (targetPlayerObject != null)
        {
            targetPlayer = targetPlayerObject.GetComponent<PlayerManager>();

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
                    gameManager.CmdAddScore(GetTargetPlayer(), -points);
                }
            }
        }
    }

    public void HitBy(int damage, string player)
    {
        if (!player.Equals(GetTargetPlayer()))
            IncreaseSpeed();
        else
            CmdReaperTookDamage(damage);
    }

    public void IncreaseSpeed()
    {
        currentSpeed += (float)shotSpeedIncrease;
    }

    public string GetTargetPlayer()
    {
        return targetPlayer.transform.name;
    }

    [Command]
    public void CmdReaperTookDamage(int damage)
    {
        RpcReaperTookDamage(damage);
    }

    [ClientRpc]
    public void RpcReaperTookDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0)
            StartCoroutine(Respawn());

    }

    public void SetSpawnPoint(Transform spawn)
    {
        spawnPoint = spawn;
    }

    public void SetPoints(int points)
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

    public void StopRespawn()
    {
        if (respawn != null)
            StopCoroutine(respawn);
    }
}
