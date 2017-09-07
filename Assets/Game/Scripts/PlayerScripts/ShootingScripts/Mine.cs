using UnityEngine;

public class Mine : MonoBehaviour
{
    public bool isProjectile;
    public GameObject triggerArea;
    Rigidbody rb;

    float speed;
    Vector3 direction;
    string playername;
    short damage;
    float lerpSpeed;
    bool collided;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(isProjectile && !collided)
        {
            rb.velocity = (transform.right * speed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Ground") || other.gameObject.layer == LayerMask.NameToLayer("Environment"))
        {
            collided = true;
            rb.velocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            triggerArea.SetActive(true);
        }
    }


    public void Local_SetProjectileVariables(float _speed, Vector3 _direction, string _playername, Vector3 hitNormal, short _damage, bool _explosive)
    {
        speed = _speed;
        direction = _direction;
        playername = _playername;
        damage = _damage;
    }

    [PunRPC]
    public void RPC_SetProjectileVariables(float _speed, Vector3 _direction, string _playername, Vector3 hitNormal, short _damage, bool _explosive)
    {
        speed = _speed;
        direction = _direction;
        playername = _playername;
        damage = _damage;
    }
}
