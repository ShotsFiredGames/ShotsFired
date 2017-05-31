using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovement : NetworkBehaviour
{
    public float rotationSpeed;
    public float jumpForce;

    [Tooltip("This will change depending on the hieght of the character.")]
    public float distToGrounded = 1.1f;
    public LayerMask ground;

    Rigidbody rb;
    GameCustomization customization;
    PlayerManager playerManager;

    float speed;
    Vector3 direction;
    Vector3 velocity;
    Quaternion rotation;
    bool crouching;
    bool isJumping;

    void Start ()
    {
        customization = GameObject.Find("GameManager").GetComponent<GameCustomization>();
        speed = customization.playerSpeed;

        rb = GetComponent<Rigidbody>();
        playerManager = GetComponent<PlayerManager>();
	}

    private void Update()
    {
        if(isJumping && Physics.Raycast(transform.position, Vector3.down, distToGrounded, ground))
        {
            playerManager.Landed();
            isJumping = false;
        }
    }

    public void Move(float horizontal, float vertical)
    {
        direction = new Vector3(horizontal * speed * Time.deltaTime, 0, vertical * speed * Time.deltaTime);
        direction = transform.TransformDirection(direction);
        rb.MovePosition(transform.position + direction);
    }

    public void Sprinting()
    {
        speed = customization.sprintSpeed;
    }

    public void StoppedSprinting()
    {
        speed = customization.playerSpeed;
    }

    public void Turn(float horizontal2)
    {
        transform.Rotate(0, horizontal2 * rotationSpeed * Time.deltaTime, 0);
    }

    public void Jump()
    {
        if(Grounded())
        {
            isJumping = true;
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }
    }

    public void Crouch()
    {
        if(!crouching)
        {
            crouching = true;
            print("I am crouching");
        }
        else
        {
            crouching = false;
            print("I am not crouching");
        }
    }

    bool Grounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, distToGrounded, ground);
    }
}
