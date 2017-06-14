using UnityEngine;
using UnityEngine.Networking;

public class Projectile : NetworkBehaviour
{
    public GameObject explosion;

    float speed;
    Vector3 direction;
    string playername;
    Rigidbody rb;

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

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.name != playername)
        {
            // other.GetComponent<CollisionDetection>().OnHit();
            explosion = Instantiate(explosion, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal)) as GameObject;
            NetworkServer.Spawn(explosion);
            NetworkServer.Destroy(gameObject);
            Destroy(gameObject);
        }
    }

    public void SetVariables(float _speed, Vector3 _direction, string _playername, Vector3 hitNormal)
    {
        speed = _speed;
        direction = _direction;
        playername = _playername;
        impactNormal = hitNormal;
    }
}
