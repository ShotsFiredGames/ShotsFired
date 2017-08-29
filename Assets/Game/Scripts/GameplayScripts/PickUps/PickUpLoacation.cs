using System.Collections;
using UnityEngine;

public class PickUpLoacation : Photon.MonoBehaviour
{
    public PickUp[] pickUpTypes;
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
        if (anim != null)
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
            Local_InstantiatePickUp(viewID, randomPickUp);
            photonView.RPC("RPC_InstantiatePickUp", PhotonTargets.OthersBuffered, viewID, randomPickUp);
        }
    }

    public void SpawnSelectPickup(string gunName)
    {
        if (waitTime != null)
            StopCoroutine(waitTime);

        if (PhotonNetwork.isMasterClient)
        {
            int viewID = PhotonNetwork.AllocateViewID();
            int pickupID = ConvertStringtoIndex(gunName);
            Local_InstantiatePickUp(viewID, pickupID);
            photonView.RPC("RPC_InstantiatePickUp", PhotonTargets.OthersBuffered, viewID, pickupID);
        }
    }

    int ConvertStringtoIndex(string gunName)
    {
        for (int index = 0; index < pickUpTypes.Length; index++)
        {
            if (pickUpTypes[index].gun.ToString().Equals(gunName))
                return index;
        }

        return 0;
    }

    void Local_InstantiatePickUp(int id, int pickUp)
    {
        activePickUp = Instantiate(pickUpTypes[pickUp].gameObject, transform.position + spawnOffset, Quaternion.identity);
        activePickUp.transform.SetParent(transform);
        activePickUp.GetComponent<PhotonView>().viewID = id;

        if (activePickUp.GetComponent<PickUp>() != null && anim != null)
            activePickUp.GetComponent<PickUp>().SetAnimator(anim);
    }

    [PunRPC]
    void RPC_InstantiatePickUp(int id, int pickUp)
    {
        activePickUp = Instantiate(pickUpTypes[pickUp].gameObject, transform.position + spawnOffset, Quaternion.identity);
        activePickUp.transform.SetParent(transform);
        activePickUp.GetComponent<PhotonView>().viewID = id;

        if (activePickUp.GetComponent<PickUp>() != null && anim != null)
            activePickUp.GetComponent<PickUp>().SetAnimator(anim);
    }

    public void Local_ActivateMimic(bool willActivate)
    {
        if (activePickUp == null)
        {
            if (PhotonNetwork.isMasterClient)
            {
                activePickUp = PhotonNetwork.Instantiate(pickUpTypes[Random.Range(0, pickUpTypes.Length)].name, transform.position + spawnOffset, Quaternion.identity, 0);
                activePickUp.transform.SetParent(transform);
            }
        }

        if (willActivate)
            activePickUp.tag.Equals("Mimic");
        else
            activePickUp.tag.Equals("Powerup");

        Rotate rotate = activePickUp.GetComponent<Rotate>();

        if (rotate == null)
            rotate = GetComponentInChildren<Rotate>();
        else
            rotate.isMimic = willActivate;
    }

    [PunRPC]
    public void RPC_ActivateMimic(bool willActivate)
    {
        if (activePickUp == null)
        {
            if (PhotonNetwork.isMasterClient)
            {
                activePickUp = PhotonNetwork.Instantiate(pickUpTypes[Random.Range(0, pickUpTypes.Length)].name, transform.position + spawnOffset, Quaternion.identity, 0);
                activePickUp.transform.SetParent(transform);
            }
        }

        if (willActivate)
            activePickUp.tag.Equals("Mimic");
        else
            activePickUp.tag.Equals("Powerup");

        Rotate rotate = activePickUp.GetComponent<Rotate>();

        if (rotate == null)
            rotate = GetComponentInChildren<Rotate>();
        else
            rotate.isMimic = willActivate;
    }

    [PunRPC]
    public void RPC_DestroyItsPickup()
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