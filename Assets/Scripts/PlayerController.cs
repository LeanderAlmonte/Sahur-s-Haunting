// using UnityEngine;

// public class FPSController : MonoBehaviour
// {
//     [Header("Movement")]
//     public float moveSpeed = 6f;
//     public float airMultiplier = 0.5f;
//     public float groundDrag = 4f;

//     [Header("Camera Look")]
//     public float sensitivity = 150f;
//     public Transform camHolder;
//     public Transform orientation;

//     private float xRotation;
//     private float horizontalInput;
//     private float verticalInput;

//     private Rigidbody rb;

//     private void Start()
//     {
//         rb = GetComponent<Rigidbody>();
//         rb.freezeRotation = true;

//         Cursor.lockState = CursorLockMode.Locked;
//         Cursor.visible = false;
//     }

//     private void Update()
//     {
//         MyInput();
//         Look();
//         ControlDrag();
//     }

//     private void FixedUpdate()
//     {
//         MovePlayer();
//     }

//     private void MyInput()
//     {
//         horizontalInput = Input.GetAxisRaw("Horizontal");
//         verticalInput = Input.GetAxisRaw("Vertical");
//     }

//     private void MovePlayer()
//     {
//         Vector3 moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
//         Vector3 force = moveDirection.normalized * moveSpeed;

//         rb.AddForce(force, ForceMode.Acceleration);
//     }

//     private void Look()
//     {
//         float mouseX = Input.GetAxisRaw("Mouse X") * sensitivity * Time.deltaTime;
//         float mouseY = Input.GetAxisRaw("Mouse Y") * sensitivity * Time.deltaTime;

//         // Horizontal rotation
//         transform.Rotate(Vector3.up * mouseX);

//         // Vertical rotation (camera only)
//         xRotation -= mouseY;
//         xRotation = Mathf.Clamp(xRotation, -85f, 85f);

//         camHolder.localRotation = Quaternion.Euler(xRotation, 0, 0);
//     }

//     private void ControlDrag()
//     {
//         rb.drag = groundDrag;
//     }
// }


/*private void Move()
{
    float h = Input.GetAxisRaw("Horizontal");
    float v = Input.GetAxisRaw("Vertical");

    Vector3 dir = orientation.forward * v + orientation.right * h;
    controller.Move(dir.normalized * speed * Time.deltaTime);

    // Gravity
    velocity.y += gravity * Time.deltaTime;
    controller.Move(velocity * Time.deltaTime);
}*/

/* private void Move()
{
 float h = Input.GetAxisRaw("Horizontal");
 float v = Input.GetAxisRaw("Vertical");

 // Movement
 Vector3 moveDir = orientation.forward * v + orientation.right * h;
 controller.Move(moveDir.normalized * speed * Time.deltaTime);

 // Gravity
  bool grounded = IsGrounded();
 if (grounded && velocity.y < 0)
     velocity.y = -2f;

 velocity.y += gravity * Time.deltaTime;
 controller.Move(velocity * Time.deltaTime);
}*/

using UnityEngine;

public class FPSController : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 6f;
    public float walkSpeed = 6f;  // walking speed
    public float runSpeed = 12f; 
    private float currentSpeed;  
    public float gravity = -9.81f;

    [Header("Camera Look")]
    public float sensitivity = 150f;
    public Transform camHolder;
    public Transform orientation;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;

    [Header("Animator")]
    public Animator animator;

    private CharacterController controller;
    private Vector3 velocity;
    private float xRotation;
    private Vector3 camOffset;

    private void Start()
    {
        controller = GetComponent<CharacterController>();

        // Load saved sensitivity from PlayerPrefs (if it exists)
        float savedSensitivity = PlayerPrefs.GetFloat("Sensitivity", sensitivity);
        sensitivity = savedSensitivity;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Store initial local position of the camera
    if (camHolder != null)
        camOffset = camHolder.localPosition;

        
    }

    private void Update()
    {
        Move();
        Look();
        Animate();
    }

private void Move()
{
    float h = Input.GetAxisRaw("Horizontal");
    float v = Input.GetAxisRaw("Vertical");

    // Determine current speed
    bool isRunning = Input.GetKey(KeyCode.LeftShift);
    float currentSpeed = isRunning ? runSpeed : walkSpeed;

    // Movement relative to orientation
    Vector3 moveDir = orientation.forward * v + orientation.right * h;

    // Apply movement
    controller.Move(moveDir.normalized * currentSpeed * Time.deltaTime);

    // Gravity
    bool grounded = IsGrounded();
    if (grounded && velocity.y < 0) velocity.y = -2f;

    velocity.y += gravity * Time.deltaTime;
    controller.Move(velocity * Time.deltaTime);
}

    private void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -85f, 85f);

        camHolder.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }

    private void Animate()
    {
        // Horizontal speed for Animator
        Vector3 horizontalVelocity = controller.velocity;
        horizontalVelocity.y = 0;
        float speed = horizontalVelocity.magnitude;

        //animator.SetFloat("CharacterSpeed", speed);
        //animator.SetBool("IsRunning", Input.GetKey(KeyCode.LeftShift) && speed > 0.1f);

        // Grounded
        bool grounded = IsGrounded();
        //animator.SetBool("IsGrounded", grounded);


        Debug.Log("Speed: " + controller.velocity.magnitude + " | Running: " + Input.GetKey(KeyCode.LeftShift));

    }

    public void SetSensitivity(float newSensitivity)
    {
        sensitivity = newSensitivity;
    }

    private bool IsGrounded()
{
    // Sphere check slightly below the player
    return Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
}
}