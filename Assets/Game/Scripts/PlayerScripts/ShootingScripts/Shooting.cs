using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Shooting : NetworkBehaviour
{
    public GameObject currentGun;

    public void Firing()
    {
        print("IsFirin' N' Stuff");
        if (currentGun != null)
            StartCoroutine(currentGun.GetComponent<Gun>().Fire());
    }

    public void SetWeapon(GameObject weapon)
    {
        currentGun = weapon;
    }

    public void RemoveWeapon()
    {
        currentGun.GetComponent<Gun>().SetActiveGun(false);
        currentGun = null;
    }
}
