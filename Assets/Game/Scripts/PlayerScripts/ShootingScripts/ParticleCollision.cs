using UnityEngine;

public class ParticleCollision : MonoBehaviour
{
    public Gun shotGun;

    short damage;

    private void Start()
    {
        damage = shotGun.damage;
    }
    
    public void DealDamage(GameObject other)
    {
        if (other.tag.Equals("Player"))
            other.GetComponentInChildren<CollisionDetection>().OnHit(damage, transform.root.name);

        if (other.tag.Equals("Reaper"))
        {
            print("ParticleCollision Hit reaper");
            other.GetComponent<Reaper>().HitBy(damage, transform.root.name);
        }
    }
}
