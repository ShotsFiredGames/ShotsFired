using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBob : MonoBehaviour
{
    [Space, Header("Movement Variables")]
    [Range(.004f, 1)]
    public float gunbobAmountX;
    [Range(.004f, 1)]
    public float gunbobAmountY;

    float currentGunbobX;
    float currentGunbobY;
    float aiming;

    HeadBob headbob;
    Shooting shooting;
    Vector3 shootingPosition;

    //Gun bob is not happening on start  
	void Start ()
    {
        headbob = transform.parent.GetComponent<HeadBob>();
        shooting = transform.root.GetComponent<Shooting>();
        aiming = 1;
	}
	
	void Update ()
    {
        currentGunbobX = Mathf.Sin(headbob.headbobStepCounter) * gunbobAmountX * aiming;
        currentGunbobY = Mathf.Cos(headbob.headbobStepCounter * 2) * gunbobAmountY * aiming;

        if(shooting.currentGun != null)
        {
            shooting.currentGun.currentRecoilZPositon = Mathf.SmoothDamp(shooting.currentGun.currentRecoilZPositon, 0, ref shooting.currentGun.currentRecoilZPositionVelocity, shooting.currentGun.recoilRecoverTime);
        }
    }

    public void ApplyRecoil()
    {
        shooting.currentGun.currentRecoilZPositon -= shooting.currentGun.recoilAmount;
    }

    public Vector3 GunBobPosition()
    {
        if (shooting.currentGun != null && aiming == 1)
            return new Vector3(currentGunbobX, currentGunbobY, 0) + new Vector3(0, 0, shooting.currentGun.currentRecoilZPositon);
        else if (shooting.currentGun != null && aiming == 0)
            return new Vector3(0, 0, shooting.currentGun.currentRecoilZPositon);
        else return Vector3.zero;
    }
    
    public void Aiming(bool isAiming)
    {
        if (!isAiming)
            aiming = 1;
        else
            aiming = 0;
    }

    //This script is not working until I aim with the machine gun, also the shooting anim does not work while aiming
}
