using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerMovement : NetworkBehaviour
{

    public float movementSpeed = 10.0f;
    public float maxVelocityChange = 10.0f;

    public float rotationSpeed;
    public float jumpForce;
    public float gravity;
    [Tooltip("This will change depending on the hieght of the character.")]
    public float distToGrounded = 1.1f;
    public LayerMask ground;
    public float juggernautSpeed;

    public float speedBoostSpeed;
    public float speedBoostDuration;

    Rigidbody rb;
    PlayerManager playerManager;
    PlayerCamera playerCamera;
    Coroutine superboots;
    Coroutine speedboost;

    float speed;
    float xRotationValue;
    float _jump;
    Quaternion rotation;
    Vector3 direction;
    Vector3 velocity;
    bool isJumping;
    bool isUsingBoots;
    float airSpeed;
    bool aimAssist;
    bool speedBoostActive;
    bool speedBoosted;
    bool landed;
    bool jumping;


    void Start ()
    {
        speed = GameCustomization.playerSpeed;
        rb = GetComponent<Rigidbody>();
        playerManager = GetComponent<PlayerManager>();
        _jump = jumpForce;
        playerCamera = GetComponent<PlayerCamera>();
        airSpeed = speed * .85f;
    }

    public void AimAssist()
    {
        if (!aimAssist)
            aimAssist = true;
    }

    public void StopAimAssist()
    {
        if (aimAssist)
            aimAssist = false;
    }

    private void FixedUpdate()
    {
        if(Grounded())
        {
            if(!landed)
            { 
                landed = true;
                jumping = false;
                playerManager.Landed();
            }

            if (speed != GameCustomization.playerSpeed && !isUsingBoots)
            {
                if(speedBoostActive || speedBoosted)
                    speed = speedBoostSpeed;
                else
                    speed = GameCustomization.playerSpeed;
            }
        }
        else
        {
            if (!jumping)
                playerManager.Falling();

            landed = false;
            if (speed != airSpeed && !isUsingBoots)
                speed = airSpeed;

            rb.velocity += Physics.gravity * gravity * Time.fixedDeltaTime;
        }
    }

    public void Move(float horizontal, float vertical)
    {
        Vector3 targetVelocity = new Vector3(horizontal, 0, vertical);
        targetVelocity = transform.TransformDirection(targetVelocity);
        targetVelocity *= speed;

        Vector3 velocity = rb.velocity;
        Vector3 velocityChange = (targetVelocity - velocity);
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0;
        rb.AddForce(velocityChange, ForceMode.VelocityChange);
        //direction = new Vector3(horizontal * speed, 0, vertical * speed);
        //direction *= Time.fixedDeltaTime;
        //direction = transform.TransformDirection(direction);
        //rb.MovePosition(transform.position + direction);
    }

    public void Turn(float horizontal2)
    {
        if (!playerCamera.isAiming)
        {

            if (!aimAssist)
            {
                xRotationValue -= -horizontal2 * rotationSpeed * Time.fixedDeltaTime;
                rotation = Quaternion.Euler(0, xRotationValue, 0);
                transform.rotation = rotation;
            }
            else
                ApplyAimAssist(horizontal2);

        }
        else
        {
            if (!aimAssist)
            {
                xRotationValue -= -horizontal2 * (rotationSpeed * .25f) * Time.fixedDeltaTime;
                rotation = Quaternion.Euler(0, xRotationValue, 0);
                transform.rotation = rotation;
            }
            else
                ApplyAimAssist(horizontal2);
        }
    }

    void ApplyAimAssist(float horizontal2)
    {
        xRotationValue -= -horizontal2 * (rotationSpeed * .2f) * Time.fixedDeltaTime;
        rotation = Quaternion.Euler(0, xRotationValue, 0);
        transform.rotation = rotation;
    }

    public void Jump()
    {
        jumping = true;
        if (Grounded())
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }
    }

    public void SuperBoots()
    {
        if (!isUsingBoots)
        {
            CancelSpeedBoost();
            superboots = StartCoroutine(MovementIncrease());
        }
    }

    public void SpeedBoosted()
    {
        CancelSpeedBoost();
        speedBoosted = true;
        speed = speedBoostSpeed;
    }

    public void ActivateSpeedBoost()
     {
         if (!speedBoostActive)
         {
            speedBoostActive = true;
            speedBoosted = false;
            speedboost = StartCoroutine(SpeedBoost());
         }
      }
 
     public void CancelSpeedBoost()
     {
         speedBoostActive = false;
         if (speedboost != null)
             StopCoroutine(speedboost);
      }


    public void CancelSuperBoots()
    {
        if (superboots != null)
        {
            StopCoroutine(superboots);

            isUsingBoots = false;
            speed = GameCustomization.playerSpeed;
            jumpForce = _jump;
        }
    }

    IEnumerator MovementIncrease()
    {
        if (!isUsingBoots)
        {
            isUsingBoots = true;
            speed = juggernautSpeed;
            jumpForce = jumpForce * 1.25f;
            yield return new WaitForSeconds(GameCustomization.abilityDuration);
            isUsingBoots = false;
            speed = GameCustomization.playerSpeed;
            jumpForce = _jump;
        }

    }

    IEnumerator SpeedBoost()
    {
        speed = speedBoostSpeed;
        yield return new WaitForSeconds(speedBoostDuration);
        if(!isUsingBoots)
        speed = GameCustomization.playerSpeed;
        speedBoostActive = false;
    }

    bool Grounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, distToGrounded, ground);
    }
}
