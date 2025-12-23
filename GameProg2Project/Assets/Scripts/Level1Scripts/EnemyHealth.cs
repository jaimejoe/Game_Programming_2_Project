using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    private Animator anim;
    private AIController aiController;

    public float corpseDespawnTime = 5f;
    private bool isDying = false;

    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        aiController = GetComponent<AIController>();
    }

    public void TakeDamage(int damage)
    {
        if (isDying) return; 

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDying = true;
        if (anim != null)
        {
            anim.Rebind();
            anim.Update(0f); 
            anim.Play("Die", 0, 0f); 
        }
        if (aiController != null)
        {
            aiController.enabled = false;
        }

        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            if (script != this && script != anim)
            {
                script.enabled = false;
            }
        }
        GetComponent<Collider>().enabled = false;
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        Destroy(gameObject, corpseDespawnTime);
    }
}