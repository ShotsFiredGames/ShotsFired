using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DestroyObject : NetworkBehaviour
{
    public float destroyAfter;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(destroyAfter);
        NetworkServer.Destroy(gameObject);
        Destroy(gameObject);
    }
}
