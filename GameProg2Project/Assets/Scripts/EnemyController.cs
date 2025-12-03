using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Animator animator;
    private EnemyHealth enemyHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    public void GotHit()
    {
        if (enemyHealth != null)
        {
            animator.SetTrigger("GotHit");
        }
    }
}
