using UnityEngine;

public class Rotate : MonoBehaviour
{
    public Vector3 rotation;
    public bool isMimic { get; set; }

    void Update()
    {
        if (!isMimic)
            transform.Rotate(rotation * Time.deltaTime);
        else
            transform.Rotate(-rotation * Time.deltaTime);
    }
}
