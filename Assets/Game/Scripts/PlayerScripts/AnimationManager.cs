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

    public void WeaponStateChanged(bool isArmed)
    {
        if(isArmed)
        {

        }
    }
    public void ApplyMovementInput(float leftStickX, float leftStickY, float rightStickX)
    {
        anim.SetFloat("LeftStickX", leftStickX);
        anim.SetFloat("LeftStickY", leftStickY);
        anim.SetFloat("RightStickX", rightStickX);
    }

    public void IsIdle() //still have to change all this

    {
        anim.SetBool("IsSprinting", false);
        anim.SetBool("IsCrouching", false);
        anim.SetBool("IsIdle", true);
    }

    public void IsCrouchIdle()
    {
        anim.SetBool("IsSprinting", false);
        anim.SetBool("IsIdle", true);
        anim.SetBool("IsCrouching", true);
    }

    public void IsMoving()
    {
        anim.SetBool("IsIdle", false);
        anim.SetBool("IsSprinting", false);
        anim.SetBool("IsCrouching", false);
    }

    public void IsSprinting()
    {
        anim.SetBool("IsIdle", false);
        anim.SetBool("IsCrouching", false);
        anim.SetBool("IsSprinting", true);
    }

    public void IsCrouching()
    {
        anim.SetBool("IsIdle", false);
        anim.SetBool("IsSprinting", false);
        anim.SetBool("IsCrouching", true);
    }

    public void IsJumping()
    {
        anim.SetBool("IsJumping", true);
    }

    public void IsLanding()
    {
        anim.SetBool("IsJumping", false);
    }
}
