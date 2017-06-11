using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Projectile : PooledObject
{
    float speed;
    Vector3 direction;
    bool isLocalBullet;
    Rigidbody rb;
    NetworkedPoolingScript objectPool;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        objectPool = GameObject.Find("GameManager").GetComponent<NetworkedPoolingScript>();
    }

    public void SetVariables(double speed, Vector3 _direction, bool isLocalBullet)
    {
        this.speed = (float)speed;
        this.direction = _direction;
        this.isLocalBullet = isLocalBullet;
        StartCoroutine((DestroyBullet(3f)));        
    }

    void Update()
    {
        if (gameObject.activeSelf)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, direction, step);
            //transform.LookAt(direction);
            //rb.velocity = (direction - transform.position).normalized * (speed * 10) * Time.deltaTime;
            //transform.position = Vector3.Lerp(transform.position, direction, speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Collision"))
        {
            if (isLocalBullet)
            {
                //other.GetComponent<CollisionDetection>().OnHit();
                print("Hit");
            }
        }
    }

    IEnumerator DestroyBullet(float waitTime)
    {
        Debug.LogError("Going to destory bullet");
        yield return new WaitForSeconds(waitTime);
        objectPool.UnSpawnObject(gameObject);
       // NetworkServer.UnSpawn(gameObject);
    }
}
