using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Variables")]
    public float rotationSpeed;

    public float staminaDrainRate;
    public float staminaGainRate;
    public Image staminaBar;

    [Space, Header("Jump Variables")]
    public LayerMask ground;
    public float distToGrounded = 1.1f;
    public float jumpForce;
    public float gravity;

    [Space, Header("Ability Variables"), Tooltip("This will change depending on the hieght of the character.")]
    public float speedBoostDuration;
    public float speedBoostSpeed;

    [HideInInspector]
    public bool isSprinting;
    [HideInInspector]
    public bool isGrounded;
    [HideInInspector]
    public bool canShake;
    [HideInInspector]
    public bool waitForShutOff;

    PlayerManager playerManager;
    PlayerCamera playerCamera;
    Rigidbody rb;

    Coroutine stopSprinting;
    Coroutine superboots;
    Coroutine speedboost;
    Coroutine sprinting;
    Coroutine checkFall;

    Quaternion rotation;

    Vector3 direction;
    Vector3 velocity;

    float maxStamina = 100;
    float xRotationValue;
    float sprintSpeed;
    float aimSpeed;
    float airSpeed;
    float stamina;
    float _jump;
    float speed;

    bool speedBoostActive;
    [HideInInspector]
    public bool juggActive;
    bool speedBoosted;
    bool checkingFall;
    bool isDraining;
    bool isJumping;
    bool aimAssist;
    bool isGaining;
    [HideInInspector]
    public bool canSprint;
    bool jumping;
    bool landed;
    bool airControlOff;

    void Start()
    {
        speed = GameCustomization.playerSpeed;
        sprintSpeed = (speed + (speed * .5f));
        aimSpeed = (speed - (speed * .5f));

        rb = GetComponent<Rigidbody>();
        playerManager = GetComponent<PlayerManager>();
        _jump = jumpForce;
        playerCamera = GetComponent<PlayerCamera>();
        airSpeed = speed * .85f;
        canSprint = true;
        stamina = maxStamina;
        staminaBar.fillAmount = stamina / maxStamina;
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
        if (Grounded())
        {
            if (isGrounded != true)
                isGrounded = true;

            if (!landed)
            {
                landed = true;
                jumping = false;
                // lockMovement = false;
                    playerManager.Landed();

                if (checkFall != null)
                {
                    StopCoroutine(checkFall);
                    canShake = false;
                    checkingFall = false;
                    airControlOff = false;
                    waitForShutOff = false;
                }
            }

            if (juggActive && speed != GameCustomization.playerSpeed)
                speed = GameCustomization.playerSpeed;

            if (speed != GameCustomization.playerSpeed && !juggActive)
            {
                if (speedBoostActive || speedBoosted)
                    speed = speedBoostSpeed;
                else
                    speed = GameCustomization.playerSpeed;
            }
        }
        else
        {
            if (isGrounded != false)
                isGrounded = false;

            if (!checkingFall)
            {
                checkingFall = true;
                checkFall = StartCoroutine(CheckFall());
            }

            if(isSprinting && !juggActive)
                playerManager.StoppedSprinting();

            playerManager.Falling();

            landed = false;
            if (speed != airSpeed && !juggActive && !airControlOff)
                speed = airSpeed;
            else if (speed != 0 && airControlOff)
                speed = 0;

            rb.velocity += Physics.gravity * gravity * Time.fixedDeltaTime;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if(!isGrounded && other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            airControlOff = true;
        }
    }

    IEnumerator CheckFall()
    {
        yield return new WaitForSeconds(1.5f);                                   
        checkingFall = false;
        canShake = true;
    }

    public void Move(float horizontal, float vertical)
    {
        if (!isSprinting && !playerCamera.isAiming)
            direction = new Vector3(horizontal * speed, 0, vertical * speed);
        else if (isSprinting && !playerCamera.isAiming)
            direction = new Vector3(horizontal * sprintSpeed, 0, vertical * sprintSpeed);
        else if (playerCamera.isAiming)
            direction = new Vector3(horizontal * aimSpeed, 0, vertical * aimSpeed);

        direction *= Time.fixedDeltaTime;
        direction = transform.TransformDirection(direction);
        rb.MovePosition(transform.position + direction);
        //rb.velocity = new Vector3(direction.x, rb.velocity.y, direction.z);
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
        if (!juggActive)
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

            juggActive = false;
            speed = GameCustomization.playerSpeed;
            jumpForce = _jump;
        }
    }

    IEnumerator MovementIncrease()
    {
        if (!juggActive)
        {
            juggActive = true;
            jumpForce = jumpForce * 1.25f;
            yield return new WaitForSeconds(GameCustomization.abilityDuration);
            juggActive = false;
            jumpForce = _jump;
        }

    }

    IEnumerator SpeedBoost()
    {
        speed = speedBoostSpeed;
        yield return new WaitForSeconds(speedBoostDuration);
        if (!juggActive)
            speed = GameCustomization.playerSpeed;
        speedBoostActive = false;
    }

    public bool Grounded()
    {
        return Physics.BoxCast(transform.position, (Vector3.one * .5f), Vector3.down, Quaternion.identity, distToGrounded, ground);
    }

    public void Sprint()
    {
        isGaining = false;
        if (stamina > 0)
        {
            isSprinting = true;
            if (stopSprinting != null)
                StopCoroutine(stopSprinting);

            canSprint = true;
        }
        else
        {
            if(!juggActive)
            isSprinting = false;
        }

        if (!staminaBar.IsActive())
            staminaBar.gameObject.SetActive(true);

        if (!juggActive && !isDraining && stamina > 0)
        {
            isDraining = true;
            sprinting = StartCoroutine(DrainStamina());
        }
    }

    IEnumerator DrainStamina()
    {
        stamina -= 5;
        staminaBar.fillAmount = stamina / maxStamina;

        if (stamina <= 0)
        {
            canSprint = false;
            if(!juggActive)
            isSprinting = false;
            isDraining = false;
            yield break;
        }

        yield return new WaitForSeconds(staminaDrainRate);
        isDraining = false;
    }

    public void StopSprint()
    {
        if (isSprinting != false)
            isSprinting = false;

        if (staminaBar.IsActive())
            staminaBar.gameObject.SetActive(false);

        if (playerManager.sprintFoV != 0)
            playerManager.sprintFoV = 0;

        if (!isGaining && stamina < maxStamina)
        {
            isGaining = true;
            stopSprinting = StartCoroutine(GainStamina());
        }
    }

    IEnumerator GainStamina()
    {
        stamina += 5;
        staminaBar.fillAmount = stamina / maxStamina;

        if (stamina >= maxStamina)
        {
            canSprint = true;
            isGaining = false;
            yield break;
        }
        yield return new WaitForSeconds(staminaGainRate);
        isGaining = false;
    }
}
