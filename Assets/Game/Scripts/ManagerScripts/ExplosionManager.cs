using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ExplosionManager : NetworkBehaviour
{
    //Version of instance taken from "http://wiki.unity3d.com/index.php/AManagerClass"
    private static ExplosionManager s_Instance = null;
    public static ExplosionManager instance
    {
        get
        {
            if (s_Instance == null)
            {
                // This is where the magic happens.
                //  FindObjectOfType(...) returns the first AManager object in the scene.
                s_Instance = FindObjectOfType(typeof(ExplosionManager)) as ExplosionManager;
            }

            // If it is still null, create a new instance
            if (s_Instance == null)
            {
                GameObject obj = new GameObject("ExplosionManager");
                s_Instance = obj.AddComponent(typeof(ExplosionManager)) as ExplosionManager;
                Debug.Log("Could not locate an ExplosionManager object. ExplosionManager was Generated Automaticly.");
            }

            return s_Instance;
        }
    }

    public GameObject explosion;
    List<GameObject> g_explosions = new List<GameObject>();

    [Command]
    public void CmdActivateExplosion(Vector3 location)
    {
        RpcActivateExplosion(location);
    }

    [ClientRpc]
    public void RpcActivateExplosion(Vector3 location)
    {
        Debug.LogError("Here to explode");
        GameObject explosionToUse = null;
        foreach (GameObject explode in g_explosions)
        {
            if (!explode.activeSelf)
            {
                explosionToUse = explode;
                break;
            }
        }

        if (explosionToUse == null)
        {
            explosionToUse = Instantiate(explosion, location, Quaternion.identity);
            g_explosions.Add(explosionToUse);
        }
        else
        {
            explosionToUse.transform.position = location;
        }
    }
}
