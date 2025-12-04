using UnityEngine;

public class AIController : MonoBehaviour
{
    public Transform player;
    public float attackRange = 1.5f;
    public float attackCooldown = 1f;

    private UnityEngine.AI.NavMeshAgent agent;
    private Animator anim;
    private float nextAttackTime = 0f;

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = GetComponent<Animator>();

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        agent.updateRotation = false;   
    }

    void Update()
    {
        if (player == null) return;

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
            }
        }

        RotateTowardPlayer();
    }

    void RotateTowardPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            if (direction.x > 0f) // Player is to the right
            {
                transform.rotation = Quaternion.Euler(0f, 90f, 0f);
            }
            else if (direction.x < 0f) // Player is to the left
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
