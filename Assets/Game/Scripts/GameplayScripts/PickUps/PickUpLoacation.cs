using System.Collections;
using UnityEngine;

public class PickUpLoacation : Photon.MonoBehaviour
{
    public GameObject[] pickUpTypes;
    public Vector3 spawnOffset;
    public float spawnDelay;
    public Animator anim;

    GameObject activePickUp;
    bool isSpawning;

    private void Start()
    {
        SpawnRandomPickup();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    void Update()
    {
        if (PhotonNetwork.isMasterClient && !isSpawning && activePickUp == null)
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
        if (PhotonNetwork.isMasterClient)
        {
            activePickUp = PhotonNetwork.Instantiate(pickUpTypes[Random.Range(0, pickUpTypes.Length)].name, transform.position + spawnOffset, Quaternion.identity, 0);

            if (activePickUp.GetComponent<PickUp>() != null)
                activePickUp.GetComponent<PickUp>().SetAnimator(anim);
        }
    }

    [PunRPC]
    public void RPC_ActivateMimic()
    {
        if (activePickUp == null)
        {
            if (PhotonNetwork.isMasterClient)
                activePickUp = PhotonNetwork.Instantiate(pickUpTypes[Random.Range(0, pickUpTypes.Length)].name, transform.position + spawnOffset, Quaternion.identity, 0);
        }

        activePickUp.tag = ("Mimic");

        Rotate rotate = activePickUp.GetComponent<Rotate>();

        if (rotate == null)
            rotate = GetComponentInChildren<Rotate>();
        else
            rotate.isMimic = true;
    }
}