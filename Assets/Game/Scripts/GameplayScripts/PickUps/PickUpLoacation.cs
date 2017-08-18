using System.Collections;
using UnityEngine;

public class PickUpLoacation : Photon.MonoBehaviour
{
    public GameObject[] pickUpTypes;
    public Vector3 spawnOffset;
    public float spawnDelay;
    public Animator anim;

    public GameObject activePickUp { get; set; }
    bool isSpawning;
    Coroutine waitTime;

    private void Start()
    {
        SpawnRandomPickup();
        anim = GetComponentInChildren<Animator>();
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
            waitTime = StartCoroutine(WaitToSpawn());
        }
    }

    IEnumerator WaitToSpawn()
    {
        yield return new WaitForSeconds(spawnDelay);
        if(anim != null)
        anim.SetBool("HasAbility", true);
        yield return new WaitForSeconds(1);
        SpawnRandomPickup();

        isSpawning = false;
    }


    public void SpawnRandomPickup()
    {
        if (PhotonNetwork.isMasterClient)
        {
            int randomPickUp = Random.Range(0, pickUpTypes.Length);

            int viewID = PhotonNetwork.AllocateViewID();
            photonView.RPC("RPC_InstantiatePickUp", PhotonTargets.AllBuffered, viewID, randomPickUp);
        }
    }

    public void SpawnSelectPickup(int pickupID)
    {
        StopCoroutine(waitTime);
        if (PhotonNetwork.isMasterClient)
        {
            int viewID = PhotonNetwork.AllocateViewID();
            photonView.RPC("RPC_InstantiatePickUp", PhotonTargets.AllBuffered, viewID, pickupID);
        }
    }

    [PunRPC]
    void RPC_InstantiatePickUp(int id, int pickUp)
    {
        foreach (GameObject pickup in pickUpTypes)
        {
            Debug.LogError(gameObject.name + " has a pickup " + activePickUp.name);
        }

        activePickUp = Instantiate(pickUpTypes[pickUp], transform.position + spawnOffset, Quaternion.identity);
        activePickUp.transform.SetParent(transform);
        activePickUp.GetComponent<PhotonView>().viewID = id;

        if (activePickUp.GetComponent<PickUp>() != null && anim != null)
            activePickUp.GetComponent<PickUp>().SetAnimator(anim);
    }
    

    [PunRPC]
    public void RPC_ActivateMimic()
    {
        if (activePickUp == null)
        {
            if (PhotonNetwork.isMasterClient)
            {
                activePickUp = PhotonNetwork.Instantiate(pickUpTypes[Random.Range(0, pickUpTypes.Length)].name, transform.position + spawnOffset, Quaternion.identity, 0);
                activePickUp.transform.SetParent(transform);
            }
        }

        activePickUp.tag = ("Mimic");

        Rotate rotate = activePickUp.GetComponent<Rotate>();

        if (rotate == null)
            rotate = GetComponentInChildren<Rotate>();
        else
            rotate.isMimic = true;
    }

    [PunRPC]
    public void RPC_DestoryItsPickup()
    {
        PhotonView pv;

        if (activePickUp != null)
        {
            pv = activePickUp.GetComponent<PhotonView>();

            if (pv != null)
                Destroy(pv.gameObject);
        }
           
    }
}