using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour {
    public Transform otherTeleporter;
    // Use this for initialization
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
            other.transform.position = new Vector3(otherTeleporter.transform.position.x, otherTeleporter.transform.position.y, otherTeleporter.transform.position.z + 1);
    }
}
