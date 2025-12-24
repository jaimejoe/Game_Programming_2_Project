using System;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    public Transform[] points; //patrol points
    public float baseSpeed = 2f;
    private int index = 0;
    Rigidbody rb;
    public ShooterEnemyHealth EnemyHealth;

    
    //stuff for chasing the player
    public bool isChasing = false;
    public Transform player;
    public float detectionRange = 20f;
    public float stopChaseRange = 30f;

    //Throwing related stuffs
    bool canThrow = true;
    public GameObject cactusPrefab;
    public GameObject fakeCactus;//its in the cowboy's hand
    public Transform throwPoint;
    public float throwForce = 20f;
    public float upwardForce = 8f;
    public float throwCooldown = 1f;
    public float throwRange = 25f;

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
        anim.SetFloat("speed",baseSpeed );
        transform.position = Vector3.MoveTowards(
                transform.position,
                points[index].position,
                baseSpeed * Time.deltaTime
                );
        transform.LookAt(points[index].position);
        //transform.Rotate(0, 90f, 0);
        if (Vector3.Distance(transform.position, points[index].position) < 0.2f)
        {
            index = (index + 1) % points.Length;
        }
    }
    void chase()
    {
        //check if he attacks
        if (Vector3.Distance(transform.position, player.position) <= throwRange)
        {
            baseSpeed = 0;
            if (canThrow)
            {
                anim.SetTrigger("attack");//the animation triggers the throwing of the cactus
                canThrow = false;
                Invoke(nameof(ResetThrow), throwCooldown);
            }
            
            //ThrowCactus();
        }else
            baseSpeed = 2;
            anim.SetFloat("speed", baseSpeed * 2);
        Vector3 playerPos = new Vector3(player.position.x, player.position.y + 0.5f, player.position.z);
        transform.position = Vector3.MoveTowards(
                transform.position,
                playerPos,
                baseSpeed*2 * Time.deltaTime
                );
        transform.LookAt(player.position);
    }
    void ThrowCactus()
    {

        GameObject cactus = Instantiate(cactusPrefab, throwPoint.position, throwPoint.rotation);
        Rigidbody crb = cactus.GetComponent<Rigidbody>();

        Vector3 dir = (player.position - throwPoint.position).normalized;

        // Arc throw
        crb.AddForce(dir * throwForce + Vector3.up * upwardForce, ForceMode.Impulse);
        Destroy(cactus,5);
        
    }
    void ResetThrow()
    {
        canThrow = true;
    }
    void ShowFakeCactus()//called in the throw animtion
    {
        fakeCactus.SetActive(true);
    }
    void HideFakeCactus()//called in the throw animtion
    {
        fakeCactus.SetActive(false);
    }
}
