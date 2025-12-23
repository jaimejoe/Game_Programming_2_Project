using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float baseWalkSpeed = 5f;
    public float speedMultiplier = 1.0f;

    [SerializeField] private AudioClip footstepClip;
    [SerializeField] private float footstepVolume = 0.8f;

    private CharacterHealth playerHealth;
    private Rigidbody rb;
    private Transform cameraTransform;
    private Animator animator;
    private AudioSource footstepSource;

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
        HandleFootsteps();
    }

    private void InitializeComponents()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        animator = GetComponent<Animator>();
        playerHealth = GetComponent<CharacterHealth>();

        footstepSource = GetComponent<AudioSource>();
        footstepSource.playOnAwake = false;
        footstepSource.loop = true;
        footstepSource.volume = footstepVolume;

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
        RotateCharacter();
        MoveCharacter();
    }

    private void CalculateMoveDirection()
    {
        if (!cameraTransform)
        {
            moveDirection = new Vector3(moveX, 0, moveZ).normalized;
            return;
        }

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        moveDirection = (forward * moveZ + right * moveX).normalized;
    }

    private void RotateCharacter()
    {
        if (IsAttacking || IsDashing) return;

        if (moveX > 0f)
            transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        else if (moveX < 0f)
            transform.rotation = Quaternion.Euler(0f, -90f, 0f);
    }

    private void MoveCharacter()
    {
        if (IsAttacking || IsDashing)
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
            groundSpeed = 0f;
            animator.SetFloat("CharacterSpeed", 0f);
            return;
        }

        float speed = baseWalkSpeed;
        groundSpeed = moveDirection != Vector3.zero ? speed : 0f;

        rb.linearVelocity = new Vector3(
            moveDirection.x * speed * speedMultiplier,
            rb.linearVelocity.y,
            moveDirection.z * speed * speedMultiplier
        );

        animator.SetFloat("CharacterSpeed", groundSpeed / baseWalkSpeed);
    }

    private void HandleFootsteps()
    {
        bool shouldPlay =
            moveDirection != Vector3.zero &&
            !IsAttacking &&
            !IsDashing;

        if (shouldPlay)
        {
            if (!footstepSource.isPlaying && footstepClip != null)
            {
                footstepSource.clip = footstepClip;
                footstepSource.Play();
            }
        }
        else
        {
            if (footstepSource.isPlaying)
            {
                footstepSource.Stop();
            }
        }
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

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            Debug.Log("Win Level 1");
        }
    }
}
