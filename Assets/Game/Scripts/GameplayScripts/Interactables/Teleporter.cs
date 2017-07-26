using UnityEngine;

public class Teleporter : MonoBehaviour {
    public Transform otherTeleporter;
    public AudioSource teleSource;
    public AudioClip teleportClip;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            if (teleSource != null)
                teleSource.PlayOneShot(teleportClip);
            other.transform.position = otherTeleporter.transform.position;
        }
    }
}
