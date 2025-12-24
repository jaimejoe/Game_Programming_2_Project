using System;
using UnityEngine;

public class CowPatrol : MonoBehaviour
{
    public Transform[] points; //patrol points
    public float baseSpeed = 4f;
    public float currentSpeed = 4f;
    private int index = 0;
    Rigidbody rb;
    public CowHp EnemyHealth;

    
    //stuff for chasing the player
    public bool isChasing = false;
    public Transform player;
    public float detectionRange = 15f;
    public float stopChaseRange = 25f;
    public float cooldown = 5f;
    bool canAttack = true;

    //animation stuff
    //Animator anim;
    public bool isAlive = true;

    //collider
    Collider collider;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        EnemyHealth = GetComponent<CowHp>();
        //anim = GetComponent<Animator>();
        collider = GetComponent<Collider>();
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) return;
        if (EnemyHealth.GetHealth() <= 0 && isAlive)
        {
            isAlive = false;
            rb.constraints = RigidbodyConstraints.None;
            rb.freezeRotation = true;
            //rb.constraints = RigidbodyConstraints.FreezePosition;
            //anim.SetBool("hasDied", true);

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
        
        
    }
    void resetAttack()// this is called in the animation
    {
        if (Vector3.Distance(transform.position, player.position) < 5f && isAlive)
        {
            
            GameManager.Instance.HealthDecrease(25);
        }
            canAttack = true;
    }
    void chase()
    {
        if (!canAttack) return;
        //check if he attacks
        if (Vector3.Distance(transform.position, player.position) < 60f)
        {

            //anim.SetTrigger("attack");
            rb.constraints = RigidbodyConstraints.FreezePosition;
            canAttack = false;
            Invoke("resetAttack", cooldown);
        }
        else
        {
            rb.constraints = RigidbodyConstraints.None;
            rb.constraints = RigidbodyConstraints.FreezePositionY;
            rb.constraints = RigidbodyConstraints.FreezeRotationX;
            rb.constraints = RigidbodyConstraints.FreezeRotationZ;
        }
            


        //anim.SetFloat("speed", currentSpeed * 2);
        Vector3 playerPos = new Vector3(player.position.x, player.position.y + 0.5f, player.position.z);
        transform.position = Vector3.MoveTowards(
                transform.position,
                playerPos,
                currentSpeed * 2 * Time.deltaTime
                );
        transform.LookAt(player.position);

        
    }

    public bool getIsChassing()
    {
        return isChasing;
    }
}
