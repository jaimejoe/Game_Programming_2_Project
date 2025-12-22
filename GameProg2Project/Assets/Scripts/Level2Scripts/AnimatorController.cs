using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;
    private CharacterMovement movement;
    
    public void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        movement = GetComponent<CharacterMovement>();
    }
    public void LateUpdate()
    {
       UpdateAnimator();
    }

    // TODO Fill this in with your animator calls
    void UpdateAnimator()
    {

        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        float speed = horizontalVelocity.magnitude;


        if (speed < 0.1f) speed = 0f;
        animator.SetFloat("speed", speed);

        animator.SetBool("isDoubleJumping", movement.isDoubleJumping);
        animator.SetBool("isFalling", !movement.IsGrounded);
        
        bool isRunning = Input.GetButton("Run");
        animator.SetBool("isRunning", isRunning);
        

    }
}
