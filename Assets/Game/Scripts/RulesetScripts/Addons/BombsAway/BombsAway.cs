using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombsAway : AddOn
{
    public List<GameObject> bombs;
    List<GameObject> availibleBombs;
    int halfOfBombs;

    void Awake()
    {
        PhotonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        availibleBombs = new List<GameObject>();
        PopulateList();
    }

    public override void StartAddOn()
    {
        PopulateList();
        StartCoroutine(SpawnBombs());
    }

    public override void EndAddOn()
    {
        ///
    }

    IEnumerator SpawnBombs()
    {
        for (int i = 0; i < bombs.Count; i++)
        {
            yield return new WaitForSeconds(.33f);
            if (PhotonNetwork.isMasterClient)
            {
                byte bombIndex = (byte)Random.Range(0, availibleBombs.Count);
                PhotonView.RPC("RPC_ActivateBomb", PhotonTargets.All, bombIndex);
            }
        }
    }

    GameObject NextBomb()
    {
        return bombs[Random.Range(0, availibleBombs.Count)];
    }

    [PunRPC]
    void RPC_ActivateBomb(byte index)
    {
        GameObject bombToActivate = bombs[index];
        bombToActivate.SetActive(true);
        availibleBombs.Remove(bombToActivate);
    }

    void PopulateList()
    {
        for (int i = 0; i < bombs.Count; i++)
        {
            if (!availibleBombs.Contains(bombs[i]))
            {
                availibleBombs.Add(bombs[i]);
            }
        }
    }
}