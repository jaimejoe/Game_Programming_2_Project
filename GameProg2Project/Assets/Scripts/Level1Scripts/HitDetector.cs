using UnityEngine;

public class HitDetector : MonoBehaviour
{
    public int damage = 20;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
                Debug.Log("Enemy took " + damage + " damage!");
            }

            AIController enemyController = other.GetComponent<AIController>();
            if (enemyController != null)
            {
                enemyController.GotHit();
            }
        }

        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.GotHit();
                Debug.Log("Player hit!");
            }
        }
    }
}