using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpLoacation : MonoBehaviour
{
    public GameObject[] pickUpTypes;
    public float spawnDelay;

    GameObject activePickUp;

    private void Start()
    {
        activePickUp = Instantiate(pickUpTypes[Random.Range(0, pickUpTypes.Length)], transform.position, Quaternion.identity) as GameObject;
    }

    void Update ()
    {
		if(activePickUp.Equals(null))
        {
            StartCoroutine(WaitToSpawn());
        }
	}

    IEnumerator WaitToSpawn()
    {
        yield return new WaitForSeconds(spawnDelay);
        activePickUp = Instantiate(pickUpTypes[Random.Range(0, pickUpTypes.Length)], transform.position, Quaternion.identity) as GameObject;
    }
}
