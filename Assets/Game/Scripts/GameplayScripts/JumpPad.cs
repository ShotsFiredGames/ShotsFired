using UnityEngine;
using System.Collections;

public class JumpPad : MonoBehaviour {

    public Vector3 liftStrength;
    public AudioSource padSource;
    public AudioClip launchClip;
    public bool lockMovement;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            if(lockMovement)
            other.GetComponent<PlayerMovement>().lockMovement = true;

            Lift(other.gameObject);
        }
    }
    private void Lift(GameObject _liftedObject)
    {
        if (_liftedObject.GetComponent<Rigidbody>())
        {
            if(padSource != null)
                padSource.PlayOneShot(launchClip);
            _liftedObject.GetComponent<Rigidbody>().velocity = liftStrength;
        }
    }
}
