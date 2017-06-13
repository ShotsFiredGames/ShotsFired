using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PickUpLoacation : NetworkBehaviour
{
    public GameObject[] pickUpTypes;
    public Vector3 spawnOffset;
    public float spawnDelay;

    GameObject activePickUp;
    bool isSpawning;

    [Server]
    private void Start()
    {
        activePickUp = Instantiate(pickUpTypes[Random.Range(0, pickUpTypes.Length)], transform.position + spawnOffset, Quaternion.identity) as GameObject;
        NetworkServer.Spawn(activePickUp);
    }

    [ServerCallback]
    void Update()
    {
        if(!isSpawning && activePickUp == null)
        {
            isSpawning = true;
            StartCoroutine(WaitToSpawn());
        }
    }

    [Server]
    IEnumerator WaitToSpawn()
    {
        yield return new WaitForSeconds(spawnDelay);
        activePickUp = Instantiate(pickUpTypes[Random.Range(0, pickUpTypes.Length)], transform.position + spawnOffset, Quaternion.identity) as GameObject;
        NetworkServer.Spawn(activePickUp);

        isSpawning = false;
    }
}
