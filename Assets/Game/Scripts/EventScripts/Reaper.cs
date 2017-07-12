using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class Reaper : NetworkBehaviour
{
    public delegate void ReaperRespawnDelegate();
    public event ReaperRespawnDelegate EventRespawn;

    [Header("Stats")]
    public float speed;
    public float respawnTime;
    public short health;
    [Header("Player Interaction")]
    public float distanceToAttack;
    public float shotSpeedIncrease;
    

    [SyncVar(hook = "OnHealthChanged")]
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

    void OnEnable()
    {
        this.EventRespawn += CallRespawn;
    }

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        Debug.LogError(netId);
    }

    [ClientCallback]
    void FixedUpdate()
    {
        targetPlayerObject = PlayerWrangler.GetPlayer(gameManager.GetWinningPlayer()).gameObject;

        if (targetPlayerObject != null)
        {
            if (targetPlayerObject.GetComponent<PlayerManager>() != targetPlayer)
            {
                targetPlayer = targetPlayerObject.GetComponent<PlayerManager>();
                targetPlayer.ActivateReaperEffects();
            }

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
                    gameManager.CallAddScore(targetPlayer.name, (short)-points);
                    targetPlayer.GetComponent<PlayerHealth>().InstantDeath("Reaper", CollisionDetection.CollisionFlag.Back);
                }
            }
        }
    }

    public void HitBy(short damage, string player)
    {
        if (!player.Equals(GetTargetPlayer()))
            IncreaseSpeed();
        else
            ReaperTookDamage(damage);
    }

    public void IncreaseSpeed()
    {
        currentSpeed += shotSpeedIncrease;
    }

    public string GetTargetPlayer()
    {
        return targetPlayer.transform.name;
    }

    [Command]
    public void CmdReaperTookDamage(short damage)
    {
        currentHealth -= damage;
    }

    public void ReaperTookDamage(short damage)
    {
        currentHealth -= damage;
    }

    [ClientRpc]
    public void RpcReaperTookDamage(short damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            //StartCoroutine(Respawn());
            EventRespawn();
        }            
    }

    void OnHealthChanged(short _health)
    {
        currentHealth = _health;

        if (currentHealth <= 0)
        {
            //StartCoroutine(Respawn());
            EventRespawn();
        }
    }

    public void SetSpawnPoint(Transform spawn)
    {
        spawnPoint = spawn;
    }

    public void SetPoints(short points)
    {
        this.points = points;
    }

    void CallRespawn()
    {
        StartCoroutine(Respawn());
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
