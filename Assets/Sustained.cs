using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sustained : MonoBehaviour
{
    Gun laserGun;
    
	void Start ()
    {
        laserGun = transform.parent.GetComponent<Gun>();
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("TriggerCollision"))
        {
            other.GetComponent<CollisionDetection>().OnHit(laserGun.damage, transform.root.name);
            Debug.LogError("Dealing Damage");
        }
    }
}
