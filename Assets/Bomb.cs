using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject fill;
    public GameObject full;
    public GameObject explode;

    private void OnEnable()
    {
        explode.SetActive(false);
        full.SetActive(true);
        fill.SetActive(true);
        fill.transform.localScale = Vector3.zero;
        StartCoroutine(Scale());
    }
    IEnumerator Scale()
    {
        Vector3 growthAmount = new Vector3(.0025f, .0025f, .0025f);
        for (float i = .0025f; i < .1f; i += .0025f)
        {
            fill.transform.localScale += growthAmount;
            yield return new WaitForSeconds(.05f);
        }
        fill.SetActive(false);
        full.SetActive(false);
        explode.SetActive(true);
    }
}
