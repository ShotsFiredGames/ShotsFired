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
    PlayerManager playerManager;
    float aiming;

    public AudioClip[] footsteps;
    public AudioSource source;
    Coroutine step;
    bool isWalking;
   
	void Start ()
    {
        playerMovement = transform.root.GetComponent<PlayerMovement>();
        playerManager = transform.root.GetComponent<PlayerManager>();
        parentLastPosition = transform.root.position;
	}

	void Update ()
    {
        if (!playerManager.canMove) return;
		if(playerMovement.isGrounded)
            headbobStepCounter += Vector3.Distance(parentLastPosition, transform.root.position) * headbobSpeed;

        transform.localPosition = new Vector3(Mathf.Sin(headbobStepCounter) * headbobAmountX , 
            (Mathf.Cos(headbobStepCounter * 2) * headbobAmountY * -1) , 0);

        parentLastPosition = transform.root.position;

        if(PlayerManager.isWalking && playerMovement.isGrounded|| PlayerManager.isSprinting && playerMovement.isGrounded)
        {
            if (!isWalking)
            {
                isWalking = true;
                step = StartCoroutine(Footsteps());
            }
        }
        else
        {
            isWalking = false;
            if (step != null)
                StopCoroutine(step);
        }

        if (!playerMovement.isGrounded)
        {
            isWalking = false;
            if (step != null)
                StopCoroutine(step);
        }
    }

    IEnumerator Footsteps()
    {
        source.clip = footsteps[Random.Range(0, footsteps.Length)];
        source.Play();
        if(PlayerManager.isWalking)
            yield return new WaitForSeconds(.5f);
        else if(PlayerManager.isSprinting)
            yield return new WaitForSeconds(.4f);
        isWalking = false;
    }

    public void Aiming(bool isAiming)
    {
        if (!isAiming)
            aiming = 1;
        else
            aiming = 0;
    }
}
