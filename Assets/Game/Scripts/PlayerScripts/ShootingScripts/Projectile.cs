using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : PooledObject
{
    float speed;

    public void SetSpeed(double speed)
    {
        this.speed = (float)speed;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.Translate(transform.forward * Time.deltaTime * speed);
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Collision"))
        {
            //other.GetComponent<CollisionDetection>().OnHit();
        }

        ReturnToPool();
    }
}
