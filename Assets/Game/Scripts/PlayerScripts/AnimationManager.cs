using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    Animator anim;

	void Start ()
    {
        anim = GetComponent<Animator>();	
	}

    public void IsIdle(float xInput)
    {
        if(!anim.GetBool("IsIdle"))
        {
            ResetAnimations();
            anim.SetBool("IsIdle", true);
        }

        anim.SetFloat("RightStickX", xInput);
    }

    public void IsMoving(float xInput, float yInput)
    {
        if(anim.GetBool("IsIdle"))
        {
            ResetAnimations();
            anim.SetBool("IsIdle", false);
        }

        anim.SetFloat("LeftStickX", xInput);
        anim.SetFloat("LeftStickY", yInput);
    }

    public void IsSprinting(float xInput, float yInput)
    {
        if (!anim.GetBool("Sprinting"))
        {
            ResetAnimations();
            anim.SetBool("Sprinting", true);
        }

        anim.SetFloat("LeftStickX", xInput);
        anim.SetFloat("LeftStickY", yInput);
    }
    
    public void StoppedSprinting()
    {
        if (anim.GetBool("Sprinting"))
        {
            anim.SetBool("Sprinting", false);
        }
    }

    public void IsJumping()
    {
        anim.SetBool("IsJumping", true);
    }

    public void IsLanding()
    {
        anim.SetBool("IsJumping", false);
    }

    public void IsCrouching(float xInput, float yInput)
    {
        if (!anim.GetBool("IsCrouching"))
        {
            ResetAnimations();
            anim.SetBool("IsCrouching", true);
        }

        anim.SetFloat("LeftStickX", xInput);
        anim.SetFloat("LeftStickY", yInput);
    }

    void ResetAnimations()
    {
        anim.SetBool("IsIdle", false);
        anim.SetBool("IsCrouching", false);
        anim.SetBool("IsSprinting", false);
    }
}
