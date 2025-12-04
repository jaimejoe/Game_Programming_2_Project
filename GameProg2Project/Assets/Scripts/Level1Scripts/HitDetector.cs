using UnityEngine;

public class HitDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyController enemyController = other.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                {
                    enemyController.GotHit();
                    Debug.Log("Calling GotHit()");
                }
            }
        }
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                {
                    playerController.GotHit();
                    Debug.Log("Calling GotHit()");
                }
            }
        }
    }
}
