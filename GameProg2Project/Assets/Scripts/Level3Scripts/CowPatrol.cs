using System;
using UnityEngine;

public class CowPatrol : MonoBehaviour
{
    public Transform[] points; //patrol points
    public float baseSpeed = 8f;
    public float currentSpeed = 8f;
    Rigidbody rb;
    public CowHp EnemyHealth;


    //stuff for chasing the player
    public bool isCharging = false;
    public Transform player;
    public float detectionRange = 70f;
    public float stopChaseRange = 70f;
    public float cooldown = 5f;
    bool canAttack = true;
    private Vector3 targetPlayerPosition;

    //animation stuff
    Animator anim;
    public bool isAlive = true;

    //collider
    //Collider collider;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        EnemyHealth = GetComponent<CowHp>();
        anim = GetComponent<Animator>();
        //collider = GetComponent<Collider>();
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void FixedUpdate()
    {
        if (isCharging)
        {
            rb.linearVelocity = transform.forward * baseSpeed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (EnemyHealth.HP <= 0)
        {
            isAlive = false;
        }
        if (!isAlive)
        {
            anim.SetBool("death", true);
            GameManager.Instance.NextLevel();
        }

        if (canAttack && !isCharging)
        {
            
            canAttack = false;
            targetPlayerPosition = player.transform.position;
            Vector3 direction = targetPlayerPosition - transform.position;
            direction.y = 0f; // keep rotation flat (optional)

            rb.rotation = Quaternion.LookRotation(direction);
            anim.SetTrigger("buildUp");

        }

    }

    void Charge()
    {
        if (!isAlive) return;
        if (isCharging) return;
        Debug.Log("WTF");
        Vector3 direction = (targetPlayerPosition - transform.position).normalized;

        targetPlayerPosition = targetPlayerPosition + direction * 100f;



        isCharging = true;
        anim.SetBool("charging", true);

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Wall")
        {
            rb.linearVelocity = Vector3.zero;
            isCharging = false;
            anim.SetBool("charging", false);
        }
        else if (collision.collider.tag == "Player")
        {
            GameManager.Instance.HealthDecrease(45);
            rb.linearVelocity = Vector3.zero;
            isCharging = false;
            anim.SetBool("charging", false);
        }

    }
    void resetCanAttack()
    {
        canAttack = true;
    }
}