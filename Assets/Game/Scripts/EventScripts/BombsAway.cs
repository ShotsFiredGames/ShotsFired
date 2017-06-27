using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombsAway : AddOn
{
    public List<GameObject> bombs;
    List<GameObject> availibleBombs;
    int halfOfBombs;

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

    IEnumerator SpawnBombs()
    {
        for(int i = 0; i < bombs.Count; i++)
        {
            yield return new WaitForSeconds(.33f);
            GameObject bombToActivate = NextBomb();
            bombToActivate.SetActive(true);
            availibleBombs.Remove(bombToActivate);
        }
    }

    GameObject NextBomb()
    {
        return bombs[Random.Range(0, availibleBombs.Count)];
    }

    void PopulateList()
    {
        for(int i = 0; i < bombs.Count; i++)
        {
            if(!availibleBombs.Contains(bombs[i]))
            {
                availibleBombs.Add(bombs[i]);
            }
        }
    }
}
