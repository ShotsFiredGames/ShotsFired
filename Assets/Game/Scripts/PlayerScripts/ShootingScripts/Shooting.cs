using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject currentGun;

    public void Firing()
    {
        if (currentGun != null)
            currentGun.GetComponent<Gun>().Fire();
    }

    public void RemoveWeapon()
    {
        currentGun.SetActive(false);
        currentGun = null;
    }
}
