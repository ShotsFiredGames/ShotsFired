using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float aimSpeed;
    public float baseFieldOfView;
    public float aimFieldOfView;

    Camera myCamera;
    GameObject currentGun;
    Vector3 basePosition;
    Vector3 aimPosition;

    public void Start()
    {
        myCamera = transform.Find("Main Camera").gameObject.GetComponent<Camera>();
    }

    public void Aim()
    {
        if(currentGun != GetComponent<Shooting>().currentGun)
        {
            currentGun = GetComponent<Shooting>().currentGun;
            basePosition = currentGun.GetComponent<Gun>().basePosition;
            aimPosition = currentGun.GetComponent<Gun>().aimPosition;
        }

        if(currentGun.transform.localPosition != aimPosition)
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, aimPosition, aimSpeed * Time.deltaTime);

        if (myCamera.fieldOfView != 20)
            myCamera.fieldOfView = Mathf.Lerp(myCamera.fieldOfView, aimFieldOfView, aimSpeed * Time.deltaTime);
    }

    public void StopAim()
    {
        if (currentGun != GetComponent<Shooting>().currentGun)
        {
            currentGun = GetComponent<Shooting>().currentGun;
            basePosition = currentGun.GetComponent<Gun>().basePosition;
            aimPosition = currentGun.GetComponent<Gun>().aimPosition;
        }

        if (currentGun.transform.localPosition != basePosition)
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, basePosition, aimSpeed * Time.deltaTime);

        if (myCamera.fieldOfView != 60)
            myCamera.fieldOfView = Mathf.Lerp(myCamera.fieldOfView, baseFieldOfView, aimSpeed * Time.deltaTime);
    }
}
