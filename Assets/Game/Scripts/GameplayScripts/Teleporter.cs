using UnityEngine;

public class Teleporter : MonoBehaviour {
    public Transform otherTeleporter;
    public Vector3 offset;
    public AudioSource teleSource;
    public AudioClip teleportClip;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            if (teleSource != null)
                teleSource.PlayOneShot(teleportClip);
            other.transform.position = new Vector3(otherTeleporter.transform.position.x + offset.x, otherTeleporter.transform.position.y + offset.y, otherTeleporter.transform.position.z + offset.z);
        }
    }
}
