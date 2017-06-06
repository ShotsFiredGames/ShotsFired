using UnityEngine;
using System.Collections;

public class JumpPad : MonoBehaviour {

    public Vector3 liftStrength;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
            Lift(other.gameObject);
    }
    private void Lift(GameObject _liftedObject)
    {
        if (_liftedObject.GetComponent<Rigidbody>())
            _liftedObject.GetComponent<Rigidbody>().velocity = liftStrength;
    }
}
