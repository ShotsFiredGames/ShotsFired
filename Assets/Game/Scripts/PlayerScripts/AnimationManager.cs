using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    Animator anim;
    bool shooting;
    Shooting shootingScript;


	void Start ()
    {
        anim = GetComponent<Animator>();
        shootingScript = GetComponent<Shooting>();
	}
        
    public void Armed()
    {
        anim.SetBool("Armed", true);
    }

    public void Disarmed()
    {
        anim.SetBool("Armed", false);
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
    }

    public void IsMoving()
    {
        anim.SetInteger("State", 2);
        anim.SetBool("IsIdle", false);
        anim.SetBool("IsSprinting", false);
    }

    public void IsSprinting()
    {
        anim.SetBool("IsSprinting", true);
        anim.SetBool("IsIdle", false);
    }

    public void StoppedSprinting()
    {
        anim.SetBool("IsSprinting", true);
    }

    public void IsJumping()
    {
        anim.SetBool("IsJumping", true);
    }

    public void IsLanding()
    {
        anim.SetBool("IsJumping", false);
    }

    public void IsFalling()
    {
        anim.SetBool("IsJumping", true);
    }

    public void IsAiming()
    {
        anim.SetInteger("State", 3);
        anim.SetBool("IsAiming", true);

        if (shootingScript.currentGun.shootingAnim == null || !shootingScript.currentGun.gameObject.activeSelf) return;
        shootingScript.currentGun.shootingAnim.SetBool("Aim", true);
    }

    public void StoppedAiming()
    {
        if (anim.GetBool("IsIdle"))
            anim.SetInteger("State", 1);
        else
            anim.SetInteger("State", 2);


        if (shootingScript.currentGun.shootingAnim == null || !shootingScript.currentGun.gameObject.activeSelf) return;
        shootingScript.currentGun.shootingAnim.SetBool("Aim", false);
    }

    public void IsFiring()
    {
        if (shooting == false)
            shooting = true;

        anim.SetBool("IsFiring", true);
    }

    public void StoppedFiring()
    {
        if (shooting == true)
            shooting = false;

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
