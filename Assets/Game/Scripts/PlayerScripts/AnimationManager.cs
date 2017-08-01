using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    Animator anim;
    Animator gunAnim;
    bool shooting;

	void Start ()
    {
        anim = GetComponent<Animator>();	
	}

    public void SetGunAnimator(Animator _gunAnim)
    {
        gunAnim = _gunAnim;
    }
    
    public void Armed()
    {
        anim.SetBool("Armed", true);
        //anim.SetLayerWeight(1, 1);
        //anim.SetLayerWeight(2, 0);
    }

    public void Disarmed()
    {
        anim.SetBool("Armed", false);
        //anim.SetLayerWeight(2, 1);
        //anim.SetLayerWeight(1, 0);
        //anim.SetLayerWeight(6, 0);
    }
    public void ApplyMovementInput(float leftStickX, float leftStickY, float rightStickX, float rightStickY)
    {
        anim.SetFloat("Horizontal", leftStickX);
        anim.SetFloat("Vertical", leftStickY);
        anim.SetFloat("Horizontal2", rightStickX);
        anim.SetFloat("Vertical2", rightStickY);
    }

    public void IsIdle()
    {
        anim.SetBool("IsSprinting", false);
        anim.SetInteger("State", 1);
        anim.SetBool("IsIdle", true);

        if (gunAnim == null || !gunAnim.gameObject.activeSelf) return;
        gunAnim.SetBool("IsIdle", true);
    }

    public void IsMoving()
    {
        anim.SetInteger("State", 2);
        anim.SetBool("IsIdle", false);
        anim.SetBool("IsSprinting", false);

        if (gunAnim == null || !gunAnim.gameObject.activeSelf) return;
        if (gunAnim.GetBool("IsAiming"))
            gunAnim.SetBool("IsIdle", true);
        else
            gunAnim.SetBool("IsIdle", false);
    }

    public void IsSprinting()
    {
        anim.SetBool("IsSprinting", true);
        anim.SetBool("IsIdle", false);

        if (gunAnim == null || !gunAnim.gameObject.activeSelf) return;

        if (gunAnim.GetBool("IsAiming"))
            gunAnim.SetBool("IsIdle", true);
        else
            gunAnim.SetBool("IsSprinting", true);
    }

    public void StoppedSprinting()
    {
        anim.SetBool("IsSprinting", true);

        if (gunAnim == null || !gunAnim.gameObject.activeSelf) return;

        if (gunAnim.GetBool("IsAiming"))
        {
            gunAnim.SetBool("IsIdle", true);
            gunAnim.SetBool("IsSprinting", false);
        }
    }

    public void IsJumping()
    {
        anim.SetBool("IsJumping", true);

        if (gunAnim == null || !gunAnim.gameObject.activeSelf) return;


        if(shooting || gunAnim.GetBool("IsAiming"))
            gunAnim.SetBool("IsJumping", false);
        else
            gunAnim.SetBool("IsJumping", true);
    }

    public void IsLanding()
    {
        anim.SetBool("IsJumping", false);

        if (gunAnim == null || !gunAnim.gameObject.activeSelf) return;
        gunAnim.SetBool("IsJumping", false);
        gunAnim.SetBool("IsFalling", false);
    }

    public void IsFalling()
    {
        anim.SetBool("IsJumping", true);
        if (gunAnim == null || !gunAnim.gameObject.activeSelf) return;

        if (gunAnim.GetBool("IsAiming"))
            gunAnim.SetBool("IsFalling", false);
        else
            gunAnim.SetBool("IsFalling", true);
    }

    public void IsAiming()
    {
        anim.SetInteger("State", 3);
        anim.SetBool("IsAiming", true);

        if (gunAnim == null || !gunAnim.gameObject.activeSelf) return;
        gunAnim.SetBool("IsAiming", true);

        gunAnim.SetBool("Stop", true);
    }

    public void StoppedAiming()
    {
        if (anim.GetBool("IsIdle"))
            anim.SetInteger("State", 1);
        else
            anim.SetInteger("State", 2);

        anim.SetBool("IsAiming", false);

        if (gunAnim == null || !gunAnim.gameObject.activeSelf) return;
        gunAnim.SetBool("IsAiming", false);
        gunAnim.SetBool("Stop", false);
    }

    public void IsFiring()
    {
        if (shooting == false)
            shooting = true;

        anim.SetBool("IsFiring", true);

        if (gunAnim == null || !gunAnim.gameObject.activeSelf) return;
        gunAnim.SetBool("IsFiring", true);
    }

    public void StoppedFiring()
    {
        if (shooting == true)
            shooting = false;

        anim.SetBool("IsFiring", false);

        if (gunAnim == null || !gunAnim.gameObject.activeSelf) return;
        gunAnim.SetBool("IsFiring", false);
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
