using UnityEngine;

public class AIController : MonoBehaviour
{
    public Transform player;
    public float attackRange = 1.5f;
    public float attackCooldown = 1f;
    public float pauseAfterAttack = 0.5f;

    private UnityEngine.AI.NavMeshAgent agent;
    private Animator anim;
    private Rigidbody rb;
    private EnemyHealth enemyHealth;
    private float nextAttackTime = 0f;
    private bool isPaused = false;
    private float pauseEndTime = 0f;
    private bool isInHitAnimation = false;
    private bool isDead = false;

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        enemyHealth = GetComponent<EnemyHealth>();

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        agent.updateRotation = false;
    }

    void Update()
    {
        if (player == null || isDead) return;

        // Don't move if currently in hit animation
        if (isInHitAnimation)
        {
            agent.isStopped = true;
            anim.SetBool("IsWalking", false);
            return;
        }

        // Check if we're in pause state after attacking
        if (isPaused)
        {
            if (Time.time >= pauseEndTime)
            {
                isPaused = false;
            }
            else
            {
                agent.isStopped = true;
                anim.SetBool("IsWalking", false);
                return;
            }
        }

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
            anim.SetBool("IsWalking", true);
        }
        else
        {
            agent.isStopped = true;
            anim.SetBool("IsWalking", false);

            if (Time.time >= nextAttackTime)
            {
                Attack();
                nextAttackTime = Time.time + attackCooldown;
                isPaused = true;
                pauseEndTime = Time.time + pauseAfterAttack;
            }
        }

        RotateTowardPlayer();
    }

    // Call this from HitDetector when enemy gets hit
    public void GotHit()
    {
        if (isInHitAnimation || isDead) return;

        StartCoroutine(HitReaction());
    }

    System.Collections.IEnumerator HitReaction()
    {
        isInHitAnimation = true;
        agent.isStopped = true;

        anim.SetTrigger("GotHit");

        // Wait for hit animation to finish
        yield return new WaitForSeconds(0.5f);

        isInHitAnimation = false;
    }

    // Called when enemy dies
    public void OnDeath()
    {
        isDead = true;

        // Stop all AI behavior
        agent.isStopped = true;
        agent.enabled = false;

        // Disable collider
        GetComponent<Collider>().enabled = false;

        // Disable this script
        enabled = false;

        Debug.Log("AIController: Enemy is dead, AI disabled");
    }

    void RotateTowardPlayer()
    {
        if (isInHitAnimation || isDead) return;

        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            if (direction.x > 0f)
            {
                transform.rotation = Quaternion.Euler(0f, 90f, 0f);
            }
            else if (direction.x < 0f)
            {
                transform.rotation = Quaternion.Euler(0f, -90f, 0f);
            }
        }
    }

    void Attack()
    {
        anim.SetTrigger("Attack");
    }
}