using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Projectile : PooledObject
{
    public float destoryTime = 5f;
    float speed;
    Vector3 direction;
    bool isLocalBullet;
    NetworkedPoolingScript objectPool;

    private void Start()
    {
        objectPool = GameObject.Find("GameManager").GetComponent<NetworkedPoolingScript>();
    }

    public void SetVariables(double speed, bool isLocalBullet)
    {
        this.speed = (float)speed;
        this.isLocalBullet = isLocalBullet;
        StartCoroutine((DestroyBullet(destoryTime)));        
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

        ReturnToPool();
    }

    IEnumerator DestroyBullet(float waitTime)
    {
        Debug.LogError("Going to destory bullet");
        yield return new WaitForSeconds(waitTime);
        objectPool.UnSpawnObject(gameObject);
       // NetworkServer.UnSpawn(gameObject);
    }
}
