using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float baseWalkSpeed = 5f;

    public float speedMultiplier = 1.0f;
    private CharacterHealth playerHealth;

    private Rigidbody rb;
    private Transform cameraTransform;
    private Animator animator;

    private float moveX;
    private float moveZ;
    private Vector3 moveDirection;

    public float groundSpeed;
    public bool IsAttacking { get; private set; }
    public bool IsDashing { get; private set; }


    private void Awake()
    {
        InitializeComponents();
    }

    private void Update()
    {
        RegisterInput();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void InitializeComponents()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        playerHealth = GetComponent<CharacterHealth>();
        animator = GetComponent<Animator>();

        if (Camera.main)
            cameraTransform = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    private void RegisterInput()
    {
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");

    }

    private void HandleMovement()
    {
        CalculateMoveDirection();
        //HandleJump();
        RotateCharacter();
        MoveCharacter();
    }

    private void CalculateMoveDirection()
    {
        if (!cameraTransform)
        {
            moveDirection = new Vector3(moveX, 0, moveZ).normalized;
        }
        else
        {
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;

            forward.y = 0f;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            moveDirection = (forward * moveZ + right * moveX).normalized;
        }
    }

    //private void HandleJump()
    //{
    //    if (jumpRequest && IsGrounded)
    //    {
    //        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    //        jumpRequest = false;
    //    }
    //}

    private void RotateCharacter()
    {
        if (IsAttacking || IsDashing) return; // Don't rotate during attack/dash

        if (Input.GetAxis("Horizontal") > 0f)
        {
            transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        }
        else if (Input.GetAxis("Horizontal") < 0f)
        {
            transform.rotation = Quaternion.Euler(0f, -90f, 0f);
        }
    }

    private void MoveCharacter()
    {
        if (IsAttacking || IsDashing)
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
            groundSpeed = 0;
            animator.SetFloat("CharacterSpeed", 0); 
            return;
        }

        float speed = baseWalkSpeed;

        groundSpeed = (moveDirection != Vector3.zero) ? speed : 0.0f;

        Vector3 newVelocity = new Vector3(
            moveDirection.x * speed * speedMultiplier,
            rb.linearVelocity.y,
            moveDirection.z * speed * speedMultiplier
        );

        rb.linearVelocity = newVelocity;

        float normalizedSpeed = groundSpeed / baseWalkSpeed;
        animator.SetFloat("CharacterSpeed", normalizedSpeed);
    }

    public void StartAttack()
    {
        IsAttacking = true;
    }

    public void EndAttack()
    {
        IsAttacking = false;
    }

    public void StartDash()
    {
        IsDashing = true;
    }

    public void EndDash()
    {
        IsDashing = false;
    }

    public void GotHit()
    {
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(10);
            animator.SetTrigger("GotHit");
        }
    }
}