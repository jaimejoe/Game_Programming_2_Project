using System;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] points; //patrol points
    public float baseSpeed = 4f;
    private int index = 0;
    public Rigidbody rb;
    public ShooterEnemyHealth EnemyHealth;

    
    //stuff for chasing the player
    public bool isChasing = false;
    public Transform player;
    public float detectionRange = 7f;
    public float stopChaseRange = 9f;

    //animation stuff
    Animator anim;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        EnemyHealth = GetComponent<ShooterEnemyHealth>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (EnemyHealth.GetHealth() <= 0)
        {
            anim.SetBool("hasDied", true);

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
        anim.SetFloat("speed",baseSpeed );
        transform.position = Vector3.MoveTowards(
                transform.position,
                points[index].position,
                baseSpeed * Time.deltaTime
                );
        transform.LookAt(points[index].position);
        transform.Rotate(0, 90f, 0);
        if (Vector3.Distance(transform.position, points[index].position) < 0.2f)
        {
            index = (index + 1) % points.Length;
        }
    }
    void chase()
    {
        anim.SetFloat("speed", baseSpeed*2);
        transform.position = Vector3.MoveTowards(
                transform.position,
                player.position,
                baseSpeed*2 * Time.deltaTime
                );
        transform.LookAt(player.position);//minor error with how he was made requires this fix
        transform.Rotate(0, 90f, 0);

        if (Vector3.Distance(transform.position, player.position) < 4f)
        {
            //insert attack here
            anim.SetBool("attack",true);

        }else
            anim.SetBool("attack", false);
    }
}
