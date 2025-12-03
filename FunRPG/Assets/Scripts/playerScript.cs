using UnityEngine;

public class playerScript : MonoBehaviour
{
    public CharacterController controller;

    public float walkSpeed = 12f;   
    // public float runSpeed = 24f;
    // public float crouchSpeed = 3f;
    public bool isMoving;
    public float gravity = -9.81f*2;
    public float jumpHeight = 2f;

    public Transform groundCheck;
    public float groundDistance = 1.0f;
    public LayerMask groundMask;

    private Animator anim;

    Vector3 velocity;
    
    bool isGrounded;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        /** Checks if touched ground, resets falling velocity. Avoid bug where falling speed increases each fall */
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        /** Right is red axis, forward is blue axis*/
        Vector3 move = transform.right*x + transform.forward*z;
        
        controller.Move(move * walkSpeed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        // set isMoving from input and update animator
        isMoving = Mathf.Abs(x) > 0.01f || Mathf.Abs(z) > 0.01f;
        HandleAnimations();
    }

    private void HandleAnimations()
    {
        if(anim) anim.SetBool("isMoving", isMoving);
    }
}