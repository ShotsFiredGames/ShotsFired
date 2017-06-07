﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : PooledObject
{
    float speed;
    Vector3 direction;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetVariables(double speed, Vector3 _direction)
    {
        this.speed = (float)speed;
        this.direction = _direction;
    }

	void Update ()
    {
        transform.LookAt(direction);
        rb.velocity = (direction - transform.position).normalized * (speed * 10) * Time.deltaTime;
        //transform.position = Vector3.Lerp(transform.position, direction, speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Collision"))
        {
            //other.GetComponent<CollisionDetection>().OnHit();
            print("Hit");
        }

        ReturnToPool();
    }
}
