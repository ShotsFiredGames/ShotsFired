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
    PlayerManager playerManager;

    float speed;
    float sprintSpeed;
    float xRotationValue;
    Quaternion rotation;
    Vector3 direction;
    Vector3 velocity;
    bool isJumping;

    void Start ()
    {
        speed = GameCustomization.playerSpeed;

        sprintSpeed = GameCustomization.sprintSpeed;

        rb = GetComponent<Rigidbody>();
        playerManager = GetComponent<PlayerManager>();
	}

    private void Update()
    {
        if(Grounded())
        {
            playerManager.Landed();
        }
    }

    public void Move(float horizontal, float vertical)
    {
        direction = new Vector3(horizontal * speed * Time.deltaTime, 0, vertical * speed * Time.deltaTime);
        direction = transform.TransformDirection(direction);
        rb.MovePosition(transform.position + direction);
    }

    public void Sprinting(float horizontal, float vertical)
    {
        direction = new Vector3(horizontal * sprintSpeed * Time.deltaTime, 0, vertical * sprintSpeed * Time.deltaTime);
        direction = transform.TransformDirection(direction);
        rb.MovePosition(transform.position + direction);
    }

    public void Turn(float horizontal2)
    {
        xRotationValue -= -horizontal2 * rotationSpeed * Time.deltaTime;
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

    bool Grounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, distToGrounded, ground);
    }
}
