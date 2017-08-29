using UnityEngine;
using System.Collections;

public class Projectile : Photon.MonoBehaviour
{
    public GameObject explosion;
    public float destroyAfter;

    float speed;
    Vector3 direction;
    string playername;
    Rigidbody rb;
    short damage;
    bool explosive;
    float lerpSpeed;

    Vector3 impactNormal; //Used to rotate impactparticle.

    IEnumerator Start()
    {
        rb = GetComponent<Rigidbody>();
        yield return new WaitForSeconds(destroyAfter);
        if (PhotonNetwork.isMasterClient)
        {
            Destroy(gameObject);
            photonView.RPC("RPC_DestroyProjectile", PhotonTargets.OthersBuffered);
        }
    }

    void FixedUpdate()
    {
        if(explosive)
        {

            lerpSpeed += speed * Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, direction, lerpSpeed);

          //  transform.LookAt(direction);
           // rb.AddForce(transform.forward * speed);
        }
        else
        {
            lerpSpeed += speed * Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, direction, lerpSpeed);
        }
    }

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
                if (PhotonNetwork.isMasterClient)
                {
                    if(explosive)
                    {
                        GameObject _explosion = PhotonNetwork.Instantiate(explosion.name, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal), 0);
                        _explosion.GetComponent<RocketExplosion>().Local_SetExplosionVariables(playername);
                        _explosion.GetComponent<PhotonView>().RPC("RPC_SetExplosionVariables", PhotonTargets.Others, playername);
                    }

                    Destroy(gameObject);
                    photonView.RPC("RPC_DestroyProjectile", PhotonTargets.OthersBuffered);
                }
            }
        }
    }

    [PunRPC]
    void RPC_DestroyProjectile()
    {
        Destroy(gameObject);
    }

    public void Local_SetProjectileVariables(float _speed, Vector3 _direction, string _playername, Vector3 hitNormal, short _damage, bool _explosive)
    {
        speed = _speed;
        direction = _direction;
        playername = _playername;
        impactNormal = hitNormal;
        damage = _damage;
        explosive = _explosive;
    }

    [PunRPC]
    public void RPC_SetProjectileVariables(float _speed, Vector3 _direction, string _playername, Vector3 hitNormal, short _damage, bool _explosive)
    {
        speed = _speed;
        direction = _direction;
        playername = _playername;
        impactNormal = hitNormal;
        damage = _damage;
        explosive = _explosive;
    }
}