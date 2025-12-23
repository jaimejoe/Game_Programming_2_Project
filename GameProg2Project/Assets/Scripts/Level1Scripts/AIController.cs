using System.Collections;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public Transform player;
    public float attackRange = 1.5f;
    public float attackCooldown = 1.5f;
    public float moveSpeed = 2f;

    private Animator anim;
    private bool isDead = false;
    private float nextAttackTime = 0f;
    private float nextMoveTime = 0f;
    private Vector3 targetPosition;

    void Start()
    {
        anim = GetComponent<Animator>();

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        nextAttackTime = Time.time + Random.Range(0f, 2f);
        PickRandomPosition();
    }

    void Update()
    {
        if (player == null || isDead) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            anim.SetBool("IsWalking", false);

            if (Time.time >= nextAttackTime)
            {
                Attack();
                nextAttackTime = Time.time + attackCooldown + Random.Range(-0.5f, 0.5f);
            }
        }
        else if (Time.time >= nextMoveTime)
        {
            anim.SetBool("IsWalking", true);

            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

            if (Vector3.Distance(transform.position, targetPosition) < 0.5f)
            {
                PickRandomPosition();
                nextMoveTime = Time.time + Random.Range(0.5f, 2f);
            }
        }
        else
        {
            anim.SetBool("IsWalking", false);
        }

        RotateTowardPlayer();
    }

    void PickRandomPosition()
    {
        float angle = Random.Range(0f, 360f);
        float distance = Random.Range(1f, 3f);

        targetPosition = player.position + new Vector3(
            Mathf.Cos(angle * Mathf.Deg2Rad) * distance,
            0,
            Mathf.Sin(angle * Mathf.Deg2Rad) * distance
        );
    }

    void Attack()
    {
        anim.SetTrigger("Attack");
        PickRandomPosition();
        nextMoveTime = Time.time + 0.3f; 
    }

    public void GotHit()
    {
        if (isDead) return;
        anim.SetTrigger("GotHit");

        PickRandomPosition();
        nextMoveTime = Time.time + 0.5f;
    }

    public void OnDeath()
    {
        isDead = true;
        anim.SetBool("IsWalking", false);
        GetComponent<Collider>().enabled = false;
        enabled = false;
    }

    void RotateTowardPlayer()
    {
        if (isDead) return;

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
}