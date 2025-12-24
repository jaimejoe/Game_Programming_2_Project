using System;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    //RUNNING/WALKING STUFF
    public float baseRunSpeed = 14f;
    public float baseWalkSpeed = 8f;
    public Transform cameraTransform;
    private bool IsRunning => Input.GetButton("Run");
    // JUMP STUFF
    public Transform groundCheck;
    public float groundDistance = 0.3f;
    public LayerMask groundMask;
    bool isGrounded;
    public float jumpHeight = 4f;

    // ANIMATION STUFF
    private Animator anim;


    private Rigidbody Rigidbody;

    void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Rigidbody.freezeRotation = true;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && Input.GetButton("Jump"))
        {
            Rigidbody.linearVelocity = new Vector3(Rigidbody.linearVelocity.x,jumpHeight, Rigidbody.linearVelocity.z);
        }
        if (Input.GetButtonDown("Fire1"))
        {
           anim.SetTrigger("Shoot0");

        }
            //if (Input.GetButtonDown("Fire1"))
            //{
            //    anim.SetBool("Shoot",true);

            //}
            //if (Input.GetButtonUp("Fire1"))
            //{
            //    anim.SetBool("Shoot", false);

            //}


            float speed = IsRunning ? baseRunSpeed : baseWalkSpeed ;
        //speed *= Time.deltaTime;
        // Get input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Get movement direction relative to camera
        Vector3 move = cameraTransform.forward * z + cameraTransform.right * x;
        move.y = 0f;
        move.Normalize();

        Vector3 newMove = move*speed;
        newMove.y = Rigidbody.linearVelocity.y;

        // Move the character
        Rigidbody.linearVelocity = newMove;
        anim.SetFloat("Speed", newMove.magnitude);
        anim.SetBool("IsGrounded", isGrounded);
        
    }
}
