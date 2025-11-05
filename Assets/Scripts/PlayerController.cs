using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;     // Movement speed

    public Transform orientation;

    float horizontalInput;
    float verticalInput;
    private Vector3 moveDirection;
    
    private float gravity = 9.81f;

    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    }

  /*  void Update()
    {
        // Get input axes
        float horizontal = Input.GetAxis("Horizontal"); // For turning
        float vertical = Input.GetAxis("Vertical");     // For moving forward/backward

        // Rotate the character based on horizontal input
        transform.Rotate(0, horizontal * turnSpeed * Time.deltaTime, 0);

        // Move the character forward/backward
        if (controller.isGrounded)
        {
            // Set move direction forward
            moveDirection = transform.forward * vertical * moveSpeed;
        }

        // Apply gravity
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the character controller
        controller.Move(moveDirection * Time.deltaTime);
    }*/
}
