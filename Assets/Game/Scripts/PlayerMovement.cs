using UnityEngine;
using UnityEngine.Networking;
using InControl;

public class PlayerMovement : NetworkBehaviour
{
    public float rotationSpeed;
    public float jumpForce;

    [Tooltip("This will change depending on the hieght of the character.")]
    public float distToGrounded = 1.1f;
    public LayerMask ground;

    Rigidbody rb;
    InputDevice inputDevice;
    GameCustomization customization;

    float speed;
    float vertical;
    float horizontal;
    float horizontal2;
    Vector3 direction;
    Vector3 velocity;
    Quaternion rotation;
    bool crouching;

    void Start ()
    {
        customization = GameObject.Find("GameManager").GetComponent<GameCustomization>();
        speed = customization.playerSpeed;

        rb = GetComponent<Rigidbody>();
	}
	
	void FixedUpdate ()
    {
        inputDevice = InputManager.ActiveDevice;

        vertical = inputDevice.LeftStickY;
        horizontal = inputDevice.LeftStickX;
        horizontal2 = inputDevice.RightStickX;

        Move();
        Turn();

        if (inputDevice.Action1.WasPressed && Grounded())
            Jump();

        if (inputDevice.LeftStickButton.IsPressed)
        {
            if(speed != customization.sprintSpeed)
                speed = customization.sprintSpeed;
        }
        else
        {
            if (speed != customization.playerSpeed)
                speed = customization.playerSpeed;
        }

        if (inputDevice.RightStick.WasPressed)
            Crouch();
	}

    void Move()
    {
        direction = new Vector3(horizontal * speed * Time.deltaTime, 0, vertical * speed * Time.deltaTime);
        transform.Translate(direction);
    }

    void Turn()
    {
        transform.Rotate(0, horizontal2 * rotationSpeed * Time.deltaTime, 0);
    }

    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
    }

    void Crouch()
    {
        if(!crouching)
        { 
            //crouch
        }
        else
        {
            //stop crouching
        }
    }

    bool Grounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, distToGrounded, ground);
    }
}
