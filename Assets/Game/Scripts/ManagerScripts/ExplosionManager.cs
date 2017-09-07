using System.Collections.Generic;
using UnityEngine;

public class ExplosionManager : MonoBehaviour
{
    #region Instance
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
    #endregion

    public GameObject explosion;
    List<GameObject> g_explosions = new List<GameObject>();

    public void Local_ActivateExplosion(Vector3 location)
    {
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

    [PunRPC]
    public void RPC_ActivateExplosion(Vector3 location)
    {
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
