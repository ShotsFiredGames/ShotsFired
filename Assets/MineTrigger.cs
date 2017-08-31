using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineTrigger : MonoBehaviour
{
    public GameObject explosion;

    AudioSource source;
    bool activated;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("TriggerCollision") && !activated)
        {
            print("Triggered");
            activated = true;
            StartCoroutine(Activated(other.gameObject));
        }
    }

    IEnumerator Activated(GameObject other)
    {
        yield return new WaitForSeconds(.33f);
        source.PlayOneShot(source.clip);
        yield return new WaitForSeconds(.33f);
        source.PlayOneShot(source.clip);
        yield return new WaitForSeconds(.33f);
        GameObject _explosion = PhotonNetwork.Instantiate(explosion.name, transform.position, Quaternion.identity, 0);
        _explosion.GetComponent<RocketExplosion>().Local_SetExplosionVariables(other.name);
        _explosion.GetComponent<PhotonView>().RPC("RPC_SetExplosionVariables", PhotonTargets.Others, other.name);
    }
}
