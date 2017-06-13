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
    
    public void Armed()
    {
        anim.SetLayerWeight(1, 1);
        anim.SetLayerWeight(2, 0);
    }

    public void Disarmed()
    {
        anim.SetLayerWeight(2, 1);
        anim.SetLayerWeight(1, 0);
        anim.SetLayerWeight(6, 0);
    }
    public void ApplyMovementInput(float leftStickX, float leftStickY, float rightStickX, float rightStickY)
    {
        anim.SetFloat("LeftStickX", leftStickX);
        anim.SetFloat("LeftStickY", leftStickY);
        anim.SetFloat("RightStickX", rightStickX);
        anim.SetFloat("RightStickY", rightStickY);
    }

    public void IsIdle()
    {
        anim.SetBool("IsSprinting", false);
        anim.SetBool("IsIdle", true);
    }

    public void IsCrouchIdle()
    {
        anim.SetBool("IsSprinting", false);
        anim.SetBool("IsIdle", true);
    }

    public void IsMoving()
    {
        anim.SetBool("IsIdle", false);
        anim.SetBool("IsSprinting", false);
    }

    public void IsJumping()
    {
        anim.SetBool("IsJumping", true);
    }

    public void IsLanding()
    {
        anim.SetBool("IsJumping", false);
    }

    public void IsAiming()
    {
        anim.SetBool("IsAiming", true);
    }

    public void StoppedAiming()
    {
        anim.SetBool("IsAiming", false);
    }

    public void IsFiring()
    {
        anim.SetBool("IsFiring", true);
    }

    public void StoppedFiring()
    {
        anim.SetBool("IsFiring", false);
    }

    public void IsDead(CollisionDetection.CollisionFlag collisionLocation)
    {
        switch (collisionLocation)                                                                                         
        {
            case CollisionDetection.CollisionFlag.FrontHeadShot:
                anim.SetInteger("Died", 1);                          
                break;
            case CollisionDetection.CollisionFlag.BackHeadShot:
                anim.SetInteger("Died", 2);
                break;
            case CollisionDetection.CollisionFlag.Front:
                anim.SetInteger("Died", 3);
                break;
            case CollisionDetection.CollisionFlag.Back:
                anim.SetInteger("Died", 4);
                break;
            case CollisionDetection.CollisionFlag.Left:
                anim.SetInteger("Died", 5);
                break;
            case CollisionDetection.CollisionFlag.Right:
                anim.SetInteger("Died", 6);
                break;
        }
    }

    public void IsRespawning()
    {
        anim.SetInteger("Died", 0);
    }
}
