using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour {
    public Transform otherTeleporter;
    public Vector3 offset;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
            other.transform.position = new Vector3(otherTeleporter.transform.position.x + offset.x, otherTeleporter.transform.position.y + offset.y, otherTeleporter.transform.position.z + offset.z);
    }
}
