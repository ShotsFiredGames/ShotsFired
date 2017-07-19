using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollision : MonoBehaviour
{
    public Gun shotGun;

    short damage;

    private void Start()
    {
        damage = shotGun.damage;
    }

    private void OnParticleCollision(GameObject other)
    {
        print(other);
        if (other.tag.Equals("Player") && other.gameObject != transform.root.gameObject)
        {
            print("Hit");
            other.GetComponentInChildren<CollisionDetection>().OnHit(damage, transform.root.name);
        }

        if (other.tag.Equals("Reaper"))
            other.GetComponent<Reaper>().HitBy(damage, transform.root.name);
    }
}
