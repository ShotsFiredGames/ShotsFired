using UnityEngine;
using UnityEngine.Networking;

public class Projectile : NetworkBehaviour
{
    public GameObject explosion;

    float speed;
    Vector3 direction;
    string playername;
    Rigidbody rb;
    short damage;

    Vector3 impactNormal; //Used to rotate impactparticle.

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        transform.LookAt(direction);
        rb.AddForce(transform.forward * speed);
    }

    [ServerCallback]
    void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.name != playername)
        {
            if(other.tag.Equals("Collision"))
                other.GetComponent<CollisionDetection>().OnHit(damage, playername);

            if (other.tag.Equals("Reaper"))
            {
                other.GetComponent<Reaper>().HitBy(damage, playername);
            }

            if (!other.tag.Equals("Juggernaut") && !other.tag.Equals("Player") && !other.tag.Equals("IgnoreCollision") && !other.tag.Equals("SpeedBoost") && !other.tag.Equals("Flag") && !other.tag.Equals("PickUp"))
            {
                explosion = Instantiate(explosion, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal)) as GameObject;
                explosion.GetComponent<RocketExplosion>().SetVariables(playername);

                NetworkServer.Spawn(explosion);
                NetworkServer.UnSpawn(gameObject);
                Destroy(gameObject);
            }
        }
    }

    public void SetVariables(float _speed, Vector3 _direction, string _playername, Vector3 hitNormal, short _damage)
    {
        speed = _speed;
        direction = _direction;
        playername = _playername;
        impactNormal = hitNormal;
        damage = _damage;
    }
}
