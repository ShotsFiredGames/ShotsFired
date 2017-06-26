using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class PickUpLoacation : NetworkBehaviour
{
    public GameObject[] pickUpTypes;
    public Vector3 spawnOffset;
    public float spawnDelay;
    public Animator anim;

    GameObject activePickUp;
    bool isSpawning;

    [ServerCallback]
    private void Start()
    {
        activePickUp = Instantiate(pickUpTypes[Random.Range(0, pickUpTypes.Length)], transform.position + spawnOffset, Quaternion.identity) as GameObject;
        NetworkServer.Spawn(activePickUp);
    }

    [ServerCallback]
    void Update()
    {
        if (!isSpawning && activePickUp == null)
        {
            isSpawning = true;
            StartCoroutine(WaitToSpawn());
        }
    }

    IEnumerator WaitToSpawn()
    {
        yield return new WaitForSeconds(spawnDelay);
        anim.SetBool("HasAbility", true);
        yield return new WaitForSeconds(1);
        activePickUp = Instantiate(pickUpTypes[Random.Range(0, pickUpTypes.Length)], transform.position + spawnOffset, Quaternion.identity) as GameObject;
        activePickUp.GetComponent<PickUp>().SetAnimator(anim);
        NetworkServer.Spawn(activePickUp);

        isSpawning = false;
    }
}