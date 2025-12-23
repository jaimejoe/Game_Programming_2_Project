using System;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] points; //patrol points
    public float baseSpeed = 4f;
    public float currentSpeed = 4f;
    private int index = 0;
    Rigidbody rb;
    public ShooterEnemyHealth EnemyHealth;

    
    //stuff for chasing the player
    public bool isChasing = false;
    public Transform player;
    public float detectionRange = 15f;
    public float stopChaseRange = 25f;
    public float cooldown = 2f;
    bool canAttack = true;

    //animation stuff
    Animator anim;
    public bool isAlive = true;

    //collider
    Collider collider;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        EnemyHealth = GetComponent<ShooterEnemyHealth>();
        anim = GetComponent<Animator>();
        collider = GetComponent<Collider>();
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (EnemyHealth.GetHealth() <= 0 && isAlive)
        {
            rb.constraints = RigidbodyConstraints.None;
            rb.freezeRotation = true;
            //rb.constraints = RigidbodyConstraints.FreezePosition;
            anim.SetBool("hasDied", true);

            //Destroy(collider);


            return;
        }
            

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if(distanceToPlayer <= detectionRange && !isChasing)
            isChasing = true;
        else if (isChasing && distanceToPlayer >= stopChaseRange)
            isChasing = false;
         
        if (isChasing)
            chase();
        else 
            patrol();
        
    }
    void patrol()
    {
        anim.SetFloat("speed", currentSpeed);
        transform.position = Vector3.MoveTowards(
                transform.position,
                points[index].position,
                currentSpeed * Time.deltaTime
                );
        transform.LookAt(points[index].position);
        //transform.Rotate(0, 90f, 0);
        if (Vector3.Distance(transform.position, points[index].position) < 0.2f)
        {
            index = (index + 1) % points.Length;
        }
    }
    void resetAttack()
    {
        canAttack = true;
    }
    void chase()
    {
        if (!canAttack) return;
        //check if he attacks
        if (Vector3.Distance(transform.position, player.position) < 4f)
        {
            
            anim.SetTrigger("attack");
            currentSpeed = 0;
            canAttack = false;
            Invoke("resetAttack", cooldown);
        }
        else
            currentSpeed = 4;


            anim.SetFloat("speed", currentSpeed * 2);
        Vector3 playerPos = new Vector3(player.position.x, player.position.y + 0.5f, player.position.z);
        transform.position = Vector3.MoveTowards(
                transform.position,
                playerPos,
                currentSpeed * 2 * Time.deltaTime
                );
        transform.LookAt(player.position);

        
    }
}
