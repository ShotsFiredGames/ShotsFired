using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBob : MonoBehaviour
{
    [Range(.004f, 1)]
    public float gunbobAmountX;
    [Range(.004f, 1)]
    public float gunbobAmountY;

    float currentGunbobX;
    float currentGunbobY;
    float aiming;

    HeadBob headbob;

	void Start ()
    {
        headbob = transform.parent.GetComponent<HeadBob>();
	}
	
	void Update ()
    {
        currentGunbobX = Mathf.Sin(headbob.headbobStepCounter) * gunbobAmountX * aiming;
        currentGunbobY = Mathf.Cos(headbob.headbobStepCounter * 2) * gunbobAmountY * aiming;
	}

    public Vector3 GunBobPosition()
    {
        return new Vector3(currentGunbobX, currentGunbobY, 0);
    }

    public void Aiming(bool isAiming)
    {
        if (!isAiming)
            aiming = 1;
        else
            aiming = 0;
    }
}
