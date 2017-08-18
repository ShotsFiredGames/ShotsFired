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
					PlayerHealth targetHealth = targetPlayer.GetComponent<PlayerHealth> ();
					targetHealth.Local_InstantDeath ("Reaper", CollisionDetection.CollisionFlag.Back);
					targetHealth.PhotonView.RPC("RPC_InstantDeath", PhotonTargets.Others, "Reaper", CollisionDetection.CollisionFlag.Back);
					if (GameManager.instance.PhotonView != null) {
						GameManager.instance.Local_AddScore (GetTargetPlayer (), (short)-points);
						GameManager.instance.PhotonView.RPC ("RPC_AddScore", PhotonTargets.Others, GetTargetPlayer (), (short)-points);
					}
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
			Local_ReaperTookDamage (damage);
			PhotonView.RPC("RPC_ReaperTookDamage", PhotonTargets.Others, damage);
        }
    }

    public void IncreaseSpeed()
    {
        currentSpeed += shotSpeedIncrease;
    }

	public void Local_ReaperTookDamage(short damage)
	{
		currentHealth -= damage;

		if (currentHealth <= 0)
			respawn = StartCoroutine(Respawn());
	}

    [PunRPC]
    public void RPC_ReaperTookDamage(short damage)
    {
        currentHealth -= damage;

//        if (currentHealth <= 0)
//            respawn = StartCoroutine(Respawn());
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
		Local_SetReaperPosition (new Vector3(-1000, -1000, -1000), GetTargetPlayer());
		PhotonView.RPC("RPC_SetReaperPosition", PhotonTargets.Others, new Vector3(-1000, -1000, -1000), GetTargetPlayer());
        isDead = true;
        yield return new WaitForSeconds(1f);
		Local_SetReaperPosition (spawnPoint, GetTargetPlayer());
		PhotonView.RPC("RPC_SetReaperPosition", PhotonTargets.Others, spawnPoint, GetTargetPlayer());
        transform.rotation = Quaternion.identity;
        currentSpeed = speed;

        currentHealth = health;
        isDead = false;
        gameObject.SetActive(true);
    }

	void Local_SetReaperPosition(Vector3 newPosition, string whichReaper)
	{
		Reaper persuer = TheReaperComes.GetReaperChasingWhom(whichReaper);

		if (persuer != null)
			persuer.transform.position = newPosition;
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
}