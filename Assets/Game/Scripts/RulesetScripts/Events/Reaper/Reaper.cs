using System.Collections;
using UnityEngine;

public class Reaper : MonoBehaviour
{
    [Header("Stats")]
    public float speed;
    public float respawnTime;
    public short health;
    [Header("Player Interaction")]
    public float distanceToAttack;
    public float shotSpeedIncrease;
    [Tooltip("The vertical offset for the players. 0 means feet.")]
    public float attackingOffset;
    public LayerMask layermask;
    public SphereCollider collisionCollider; 

    public PhotonView PhotonView { get; private set; }

    short currentHealth;
    short points;
    bool isDead;

    private Vector3 spawnPoint;
    private float currentDistance;
    private float currentSpeed;
    PlayerManager targetPlayer;
    GameObject targetPlayerObject;
    GameManager gameManager;
    Coroutine respawn;

    void Awake()
    {
        PhotonView = GetComponent<PhotonView>();
    }

    private void OnEnable()
    {
        collisionCollider.enabled = true;
    }

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
                    targetPlayer.GetComponent<PlayerHealth>().PhotonView.RPC("RPC_InstantDeath", PhotonTargets.All, "Reaper", CollisionDetection.CollisionFlag.Back);
                    if (GameManager.instance.PhotonView != null)
                        GameManager.instance.PhotonView.RPC("RPC_AddScore", PhotonTargets.All, GetTargetPlayer(), (short)-points);
                    currentSpeed = speed;
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
            PhotonView.RPC("RPC_ReaperTookDamage", PhotonTargets.All, damage);
            print("taking damage");
        }
    }

    public void IncreaseSpeed()
    {
        currentSpeed += shotSpeedIncrease;
    }

    [PunRPC]
    public void RPC_ReaperTookDamage(short damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
            respawn = StartCoroutine(Respawn());
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
        spawnPoint = spawn.position;
    }

    public void SetPoints(short points)
    {
        this.points = points;
    }

    IEnumerator Respawn()
    {
        RefereeManager.instance.PlayReaperDies();
        PhotonView.RPC("RPC_SetReaperPosition", PhotonTargets.All, new Vector3(-1000, -1000, -1000), GetTargetPlayer());
        isDead = true;
        yield return new WaitForSeconds(1f);
        PhotonView.RPC("RPC_SetReaperPosition", PhotonTargets.All, spawnPoint, GetTargetPlayer());
        transform.rotation = Quaternion.identity;
        currentSpeed = speed;

        currentHealth = health;
        isDead = false;
        gameObject.SetActive(true);
    }

    [PunRPC]
    void RPC_SetReaperPosition(Vector3 newPosition, string whichReaper)
    {
        Reaper persuer = TheReaperComes.GetReaperChasingWhom(whichReaper);

        if (persuer != null)
            persuer.transform.position = newPosition;
    }

    public void StopReaper()
    {
        if (respawn != null)
            StopCoroutine(respawn);

        targetPlayer.ReaperEffectsActivate(false);
        gameObject.SetActive(false);
        this.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == layermask)
        {
            collisionCollider.enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == layermask)
        {
            collisionCollider.enabled = true;
        }
    }
}