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

using UnityEngine;

public class FPSController : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 6f;
    public float gravity = -9.81f;

    [Header("Camera Look")]
    public float sensitivity = 150f;
    public Transform camHolder;
    public Transform orientation;

    private CharacterController controller;
    private Vector3 velocity;
    private float xRotation;

    private void Start()
    {
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Move();
        Look();
    }

    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = orientation.forward * v + orientation.right * h;
        controller.Move(dir.normalized * speed * Time.deltaTime);

        // Gravity
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
}
