using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private PlayerController movement;
    private Rigidbody rb;
    void Start()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        animator.SetFloat("CharacterSpeed",rb.linearVelocity.magnitude);
    }
}
