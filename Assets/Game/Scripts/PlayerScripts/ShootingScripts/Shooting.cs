using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject currentGun;

    public void Firing()
    {
        print("IsFirin' N' Stuff");
        //if (currentGun != null)
            StartCoroutine(currentGun.GetComponent<Gun>().Fire());
    }

    public void SetWeapon(GameObject weapon)
    {
        currentGun = weapon;
    }

    public void RemoveWeapon()
    {
        currentGun.SetActive(false);
        currentGun = null;
    }
}
