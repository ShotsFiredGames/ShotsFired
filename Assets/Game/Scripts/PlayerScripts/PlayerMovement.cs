using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerMovement : NetworkBehaviour
{
    public float rotationSpeed;
    public float jumpForce;
    public float gravity;
    [Tooltip("This will change depending on the hieght of the character.")]
    public float distToGrounded = 1.1f;
    public LayerMask ground;

    Rigidbody rb;
    PlayerManager playerManager;
    PlayerCamera playerCamera;

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

    void Start ()
    {
        speed = GameCustomization.playerSpeed;
        rb = GetComponent<Rigidbody>();
        playerManager = GetComponent<PlayerManager>();
        _jump = jumpForce;
        playerCamera = GetComponent<PlayerCamera>();

        airSpeed = speed * .5f;
	}

    public void AimAssist()
    {
        if(!aimAssist)
        aimAssist = true;
    }

    public void StopAimAssist()
    {
        if(aimAssist)
        aimAssist = false;
    }

    private void Update()
    {
        if(Grounded())
        {
            playerManager.Landed();

            if (speed != GameCustomization.playerSpeed && !isUsingBoots)
            {
                speed = GameCustomization.playerSpeed;
            }
        }
        else
        {
            if (speed != airSpeed)
                speed = airSpeed;

            rb.velocity += Physics.gravity * gravity * Time.fixedDeltaTime;
        }
    }

    public void Move(float horizontal, float vertical)
    {
        direction = new Vector3(horizontal * speed * Time.deltaTime, 0, vertical * speed * Time.deltaTime);
        direction = transform.TransformDirection(direction);
        rb.MovePosition(transform.position + direction);
    }

    public void Turn(float horizontal2)
    {
        if(!playerCamera.isAiming)
        {

            if (!aimAssist)
            {
                xRotationValue -= -horizontal2 * rotationSpeed * Time.deltaTime;
                rotation = Quaternion.Euler(0, xRotationValue, 0);
                transform.rotation = rotation;
            }
            else
                ApplyAimAssist(horizontal2);

        }
        else
        {
            if(!aimAssist)
            {
                xRotationValue -= -horizontal2 * (rotationSpeed * .25f) * Time.deltaTime;
                rotation = Quaternion.Euler(0, xRotationValue, 0);
                transform.rotation = rotation;
            }
            else
            ApplyAimAssist(horizontal2);
        }
    }

    void ApplyAimAssist(float horizontal2)
    {
        xRotationValue -= -horizontal2 * (rotationSpeed * .1f) * Time.deltaTime;
        rotation = Quaternion.Euler(0, xRotationValue, 0);
        transform.rotation = rotation;
    }

    public void Jump()
    {
        if (Grounded())
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }
    }

    public void SuperBoots()
    {
        StartCoroutine(BootsAbilty());
    }

    IEnumerator BootsAbilty()
    {
        if (!isUsingBoots)
        {
            isUsingBoots = true;
            speed = speed * 1.5f;
            jumpForce = jumpForce * 1.5f;
            yield return new WaitForSeconds(GameCustomization.abilityDuration);
            isUsingBoots = false;
            speed = GameCustomization.playerSpeed;
            jumpForce = _jump;
        }

    }

    bool Grounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, distToGrounded, ground);
    }
}
