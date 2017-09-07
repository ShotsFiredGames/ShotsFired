using UnityEngine;

public class ParticleHit : MonoBehaviour
{
    public ParticleCollision particleCollision;

    private void OnParticleCollision(GameObject other)
    {
        if(other.tag.Equals("Player") && other.gameObject != transform.root.gameObject)
            particleCollision.DealDamage(other);

        if (other.tag.Equals("Reaper"))
        {
            print("Hit the reaper");
            particleCollision.DealDamage(other);
        }
    }
}
