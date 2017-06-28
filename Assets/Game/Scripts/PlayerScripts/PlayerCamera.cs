﻿using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float lookSpeed;
    public float clampValue;
    public float aimSpeed;
    public float baseFieldOfView;
    public float aimFieldOfView;

    public GameObject cameraObject;
    public Camera myCamera;
    GameObject currentGun;
    Vector3 basePosition;
	Vector3 baseRotation;
    Vector3 aimPosition;
	Vector3 aimRotation;
    PlayerManager playerManager;

    float xRotationValue;
    float yRotationValue;
    float rotationSpeed;
    Quaternion cameraYRotation;
    [HideInInspector]
    public bool isAiming;

    void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
    }

    public void Start()
    {
        rotationSpeed = GetComponent<PlayerMovement>().rotationSpeed;        
    }

    public void Look(float rightStickY)
    {
        if(!isAiming)
            yRotationValue += -rightStickY * lookSpeed * Time.fixedDeltaTime;
        else
            yRotationValue += -rightStickY * (lookSpeed * .25f) * Time.fixedDeltaTime;

        yRotationValue = ClampAngle(yRotationValue, -clampValue, clampValue);
        cameraYRotation = Quaternion.Euler(yRotationValue, 0, 0);
        SendRotationValue(yRotationValue);
        cameraObject.transform.rotation = cameraYRotation;
        cameraObject.transform.localRotation = Quaternion.Slerp(myCamera.transform.localRotation, cameraYRotation, 1);
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
        if (!isAiming)
            isAiming = true;

        GetGun();
        if (currentGun != null)
        {
            if (currentGun.transform.localPosition != aimPosition)
              currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, aimPosition, aimSpeed * Time.fixedDeltaTime);

			if (currentGun.transform.localRotation != Quaternion.Euler(aimRotation))
				currentGun.transform.localRotation = Quaternion.Lerp(currentGun.transform.localRotation, Quaternion.Euler(aimRotation), aimSpeed * Time.fixedDeltaTime);

			if (myCamera.fieldOfView != aimFieldOfView)
                myCamera.fieldOfView = Mathf.Lerp(myCamera.fieldOfView, aimFieldOfView, aimSpeed * Time.fixedDeltaTime);
        }
    }

    public void StopAim()
    {
        if (isAiming)
            isAiming = false;

        GetGun();
        if (currentGun != null)
        {
            if (currentGun.transform.localPosition != basePosition)
				currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, basePosition, aimSpeed * Time.fixedDeltaTime);

			if (currentGun.transform.localRotation != Quaternion.Euler(baseRotation))
				currentGun.transform.localRotation = Quaternion.Lerp(currentGun.transform.localRotation, Quaternion.Euler(baseRotation), aimSpeed * Time.fixedDeltaTime);

			if (myCamera.fieldOfView != baseFieldOfView)
                myCamera.fieldOfView = Mathf.Lerp(myCamera.fieldOfView, baseFieldOfView, aimSpeed * Time.fixedDeltaTime);
        }
    }

    public void SetFieldOfView(int view)
    {
        if(myCamera == null)
        {
            myCamera = transform.Find("Camera").gameObject.GetComponent<Camera>();
        }
        myCamera.fieldOfView = view;
    }

    void GetGun()
    {
        if (currentGun != GetComponent<Shooting>().currentGun)
        {
            currentGun = GetComponent<Shooting>().currentGun.gameObject;
            if (currentGun == null) return;

            basePosition = currentGun.GetComponent<Gun>().basePosition;
			baseRotation = currentGun.GetComponent<Gun> ().baseRotation;
            aimPosition = currentGun.GetComponent<Gun>().aimPosition;
			aimRotation = currentGun.GetComponent<Gun> ().aimRotation;
        }
    }
}
