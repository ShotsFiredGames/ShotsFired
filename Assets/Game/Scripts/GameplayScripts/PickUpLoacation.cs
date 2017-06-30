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
        SpawnRandomPickup();
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
        SpawnRandomPickup();

        isSpawning = false;
    }

    public void SpawnRandomPickup()
    {
        activePickUp = Instantiate(pickUpTypes[Random.Range(0, pickUpTypes.Length)], transform.position + spawnOffset, Quaternion.identity) as GameObject;

        if (activePickUp.GetComponent<PickUp>() != null)
            activePickUp.GetComponent<PickUp>().SetAnimator(anim);

        NetworkServer.Spawn(activePickUp);
    }

    [Command]
    public void CmdActivateMimic()
    {
        if (activePickUp.GetComponent<PickUp>() == null)
        {
            activePickUp = Instantiate(pickUpTypes[Random.Range(0, pickUpTypes.Length)], transform.position + spawnOffset, Quaternion.identity) as GameObject;
            NetworkServer.Spawn(activePickUp);
        }

        activePickUp.tag = ("Mimic");

        Rotate rotate = activePickUp.GetComponent<Rotate>();

        if (rotate == null)
            rotate = GetComponentInChildren<Rotate>();

        rotate.isMimic = true;
    }

    void UnspawnPickup()
    {
        NetworkServer.UnSpawn(activePickUp);
        Destroy(activePickUp);
    }
}