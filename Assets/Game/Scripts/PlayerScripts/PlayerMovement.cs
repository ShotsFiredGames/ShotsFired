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
    [HideInInspector]
    public bool juggActive;
    [HideInInspector]
    public bool canSprint;

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
    float defaultSpeed;
    float stamina;
    float _jump;
    float speed;

    bool speedBoostActive;
    bool speedBoosted;
    bool checkingFall;
    bool isDraining;
    bool isJumping;
    bool aimAssist;
    bool isGaining;
    bool jumping;
    bool landed;
    bool airControlOff;

    void Start()
    {
        SetSpeed((byte)GameCustomization.playerSpeed);
        rb = GetComponent<Rigidbody>();
        playerManager = GetComponent<PlayerManager>();
        _jump = jumpForce;
        playerCamera = GetComponent<PlayerCamera>();
        canSprint = true;
        stamina = maxStamina;
        staminaBar.fillAmount = stamina / maxStamina;
    }

    public void SetSpeed(byte speedToSet)
    {
        defaultSpeed = speedToSet;
        speed = speedToSet;
        sprintSpeed = (speedToSet + (speedToSet * .5f));
        aimSpeed = (speedToSet - (speedToSet * .5f));
        airSpeed = speedToSet * .85f;
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

            if (isSprinting && !juggActive)
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
        if (!isGrounded && other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if (Mathf.Abs(Physics.gravity.y) >= 9.8f)
                airControlOff = true;
        }
    }

    IEnumerator CheckFall()
    {
        yield return new WaitForSeconds(1.5f);
        checkingFall = false;

        if (Mathf.Abs(Physics.gravity.y) >= 9.8f)
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
            if (!juggActive)
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
            if (!juggActive)
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