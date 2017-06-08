using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour {
    public Transform otherTeleporter;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
            other.transform.position = new Vector3(otherTeleporter.transform.position.x, otherTeleporter.transform.position.y, otherTeleporter.transform.position.z + 4);
    }
}
