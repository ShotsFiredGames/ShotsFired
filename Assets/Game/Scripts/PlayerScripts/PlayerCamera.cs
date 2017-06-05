using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float lookSpeed;
    public float clampValue;
    public float aimSpeed;
    public float baseFieldOfView;
    public float aimFieldOfView;

    Camera myCamera;
    GameObject currentGun;
    Vector3 basePosition;
    Vector3 aimPosition;
    PlayerManager playerManager;

    float xRotationValue;
    float yRotationValue;
    float rotationSpeed;
    Quaternion cameraYRotation;

    public void Start()
    {
        myCamera = transform.Find("Main Camera").gameObject.GetComponent<Camera>();
        rotationSpeed = GetComponent<PlayerMovement>().rotationSpeed;
        playerManager = GetComponent<PlayerManager>();
    }

    public void Look(float rightStickY, float righStickX)
    {
        xRotationValue -= -righStickX * rotationSpeed * Time.deltaTime;
        yRotationValue += -rightStickY * lookSpeed * Time.deltaTime;
        yRotationValue = ClampAngle(yRotationValue, -clampValue, clampValue);
        cameraYRotation = Quaternion.Euler(yRotationValue, xRotationValue, 0);
        SendRotationValue(yRotationValue);
        myCamera.transform.rotation = cameraYRotation;
    }

    void SendRotationValue(float value)
    {
        playerManager.SetRotationValue(value);
    }

    public static float ClampAngle(float angle, float min, float max)              
    {
        if (angle < -360.0f)
            angle += 360.0f;
        if (angle > 360.0f)
            angle -= 360.0f;
        return Mathf.Clamp(angle, min, max);
    }

    public void Aim()
    {
        GetGun();

        if (currentGun != null)
        {
            if (currentGun.transform.localPosition != aimPosition)
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, aimPosition, aimSpeed * Time.deltaTime);

            if (myCamera.fieldOfView != 20)
                myCamera.fieldOfView = Mathf.Lerp(myCamera.fieldOfView, aimFieldOfView, aimSpeed * Time.deltaTime);
        }

        
    }

    public void StopAim()
    {
        GetGun();

        if (currentGun != null)
        {
            if (currentGun.transform.localPosition != basePosition)
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, basePosition, aimSpeed * Time.deltaTime);

            if (myCamera.fieldOfView != 60)
                myCamera.fieldOfView = Mathf.Lerp(myCamera.fieldOfView, baseFieldOfView, aimSpeed * Time.deltaTime);

        }
    }

    public void SetFieldOfView(int view)
    {
        if(myCamera == null)
        {
            myCamera = transform.Find("Main Camera").gameObject.GetComponent<Camera>();
        }
        myCamera.fieldOfView = view;
    }

    void GetGun()
    {
        if (currentGun != GetComponent<Shooting>().currentGun)
        {
            currentGun = GetComponent<Shooting>().currentGun;
            if (currentGun == null) return;

            basePosition = currentGun.GetComponent<Gun>().basePosition;
            aimPosition = currentGun.GetComponent<Gun>().aimPosition;
        }
    }
}
