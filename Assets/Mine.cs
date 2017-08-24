using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public bool isProjectile;

    Vector3 direction;
    string playerName;
    float speed;
    short damage;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (isProjectile)
            rb.AddForce(direction * speed, ForceMode.VelocityChange);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("CollisionTrigger"))
        {
            other.GetComponent<CollisionDetection>().OnHit(damage, playerName);
        }
    }

    [PunRPC]
    public void RPC_SetProjectileVariables(float _speed, Vector3 _direction, string _playername, Vector3 hitNormal, short _damage, bool _explosive)
    {
        speed = _speed;
        direction = _direction;
        playerName = _playername;
        damage = _damage;
    }
}
