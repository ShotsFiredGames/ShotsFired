using UnityEngine;
using UnityEngine.Networking;

public class Projectile : NetworkBehaviour
{
    public GameObject explosion;

    float speed;
    Vector3 direction;
    string playername;
    Rigidbody rb;
    int damage;

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
        }

        if(!other.tag.Equals("Juggernaut") && !other.tag.Equals("Player") && !other.tag.Equals("IgnoreCollision") && !other.tag.Equals("Flag"))
        {
            explosion = Instantiate(explosion, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal)) as GameObject;
            explosion.GetComponent<RocketExplosion>().SetVariables(playername);

            NetworkServer.Spawn(explosion);
            NetworkServer.UnSpawn(gameObject);
            Destroy(gameObject);
        }
    }

    public void SetVariables(float _speed, Vector3 _direction, string _playername, Vector3 hitNormal, int _damage)
    {
        speed = _speed;
        direction = _direction;
        playername = _playername;
        impactNormal = hitNormal;
        damage = _damage;
    }
}
