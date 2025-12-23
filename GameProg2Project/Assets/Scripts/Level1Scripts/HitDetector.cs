using UnityEngine;

public class HitDetector : MonoBehaviour
{
    public int damage = 20;
    public AudioClip hitSound;
    public float hitVolume = 0.5f; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (hitSound != null)
            {
                AudioSource.PlayClipAtPoint(hitSound, other.transform.position, hitVolume);
            }

            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }

            AIController enemyController = other.GetComponent<AIController>();
            if (enemyController != null)
            {
                enemyController.GotHit();
            }

            Debug.Log($"Hit enemy: {other.name}");
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
