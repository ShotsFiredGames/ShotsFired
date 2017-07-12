using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour
{
    public float lookSpeed;
    public float clampValue;
    public float aimSpeed;
    public float baseFieldOfView;
    public float aimFieldOfView;

    public GameObject cameraObject;
    public Camera myCamera;
    GameObject gunHolder;
    GameObject currentGun;
    Vector3 basePosition;
	Vector3 baseRotation;
    Vector3 aimPosition;
	Vector3 aimRotation;
    PlayerManager playerManager;

    float xRotationValue;
    float yRotationValue;
    Quaternion cameraYRotation;
    [HideInInspector]
    public bool isAiming;
    Coroutine currentAimCo;

    void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
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
        if(currentAimCo != null)
            StopMyCoroutine(currentAimCo);
        SetView(aimPosition, aimRotation, aimFieldOfView);
    }

    public void StopAim()
    {
        if (isAiming)
            isAiming = false;

        GetGun();
        if (currentAimCo != null)
            StopMyCoroutine(currentAimCo);
        SetView(basePosition, baseRotation, baseFieldOfView);
    }

    public void SetFieldOfView(int view)
    {
        if(myCamera == null)
            myCamera = transform.Find("Camera").gameObject.GetComponent<Camera>();
        myCamera.fieldOfView = view;
    }

    void GetGun()
    {
        if (currentGun != GetComponent<Shooting>().currentGun)
        {
            gunHolder = GetComponent<Shooting>().gunHolder;
            currentGun = GetComponent<Shooting>().currentGun.gameObject;
            if (currentGun == null) return;

            basePosition = currentGun.GetComponent<Gun>().basePosition;
			baseRotation = currentGun.GetComponent<Gun> ().baseRotation;
            aimPosition = currentGun.GetComponent<Gun>().aimPosition;
			aimRotation = currentGun.GetComponent<Gun> ().aimRotation;
        }
    }

    void SetView(Vector3 newPos, Vector3 newRot, float newFOV)
    {
        currentAimCo = StartCoroutine(MoveToPos(newPos, newRot, newFOV));
    }

    bool moving; //Variable that should only be used in the MoveToPos coroutine

    IEnumerator MoveToPos(Vector3 newPos, Vector3 newRot, float newFOV)
    {
        if(!moving)
        {
            moving = true;
            yield return new WaitUntil(() => InPosition(newPos, newRot, newFOV) == true);
            moveSpeed = 0;
            atPos = false;
            atRot = false;
            atFOV = false;
            moving = false;
        }
    }

    void StopMyCoroutine(Coroutine aCoroutine)
    {
        if(aCoroutine != null)
        {
            StopCoroutine(aCoroutine);
            moveSpeed = 0;
            atPos = false;
            atRot = false;
            atFOV = false;
            moving = false;
        }
    }

    bool atPos, atRot, atFOV; //Variables that should really only be used in the InPosition function
    float moveSpeed;

    bool InPosition(Vector3 newPos, Vector3 newRot, float newFOV)
    {
        if (gunHolder != null)
        {
            moveSpeed += aimSpeed * Time.fixedDeltaTime;
            if (Vector3.Distance(gunHolder.transform.localPosition, newPos) > 0.01f)
                gunHolder.transform.localPosition = Vector3.Lerp(gunHolder.transform.localPosition, newPos, moveSpeed);
            else
            {
                gunHolder.transform.localPosition = newPos;
                atPos = true;
            }

            if (Quaternion.Angle(gunHolder.transform.localRotation, Quaternion.Euler(newRot)) > 1)
                gunHolder.transform.localRotation = Quaternion.Lerp(gunHolder.transform.localRotation, Quaternion.Euler(newRot), moveSpeed);
            else
            {
                gunHolder.transform.localRotation = Quaternion.Euler(newRot);
                atRot = true;
            }

            if (myCamera.fieldOfView != newFOV)
                myCamera.fieldOfView = Mathf.Lerp(myCamera.fieldOfView, newFOV, moveSpeed);
            else
            {
                myCamera.fieldOfView = newFOV;
                atFOV = true;
            }

            if (atPos && atRot && atFOV)
                return true;
            else
                return false;
        }

        return false;
    }
}
