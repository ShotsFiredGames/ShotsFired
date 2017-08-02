using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBob : MonoBehaviour
{
    public float headbobSpeed;
    public float headbobAmountX;
    public float headbobAmountY;
    [HideInInspector]
    public float headbobStepCounter;
    Vector3 parentLastPosition;
    PlayerMovement playerMovement;
    float aiming;
   
	void Start ()
    {
        playerMovement = transform.root.GetComponent<PlayerMovement>();
        parentLastPosition = transform.root.position;
	}

	void Update ()
    {
		if(playerMovement.isGrounded)
            headbobStepCounter += Vector3.Distance(parentLastPosition, transform.root.position) * headbobSpeed;

        transform.localPosition = new Vector3(Mathf.Sin(headbobStepCounter) * headbobAmountX , 
            (Mathf.Cos(headbobStepCounter * 2) * headbobAmountY * -1) , 0);

        parentLastPosition = transform.root.position;
	}

    public void Aiming(bool isAiming)
    {
        if (!isAiming)
            aiming = 1;
        else
            aiming = 0;
    }
}
