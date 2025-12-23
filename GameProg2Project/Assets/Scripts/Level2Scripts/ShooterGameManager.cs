using UnityEngine;

public class ShooterGameManager : MonoBehaviour
{
    public static ShooterGameManager Instance;

    public int enemiesAlive = 0;

    void Awake()
    {
        Instance = this;
    }

    public void EnemySpawned()
    {
        enemiesAlive++;
    }

    public void EnemyDied()
    {
        enemiesAlive--;

        Debug.Log("Enemies left: " + enemiesAlive);

        if (enemiesAlive <= 0)
        {
            Debug.Log("ALL ENEMIES DEAD");
            // unlock door / load next level / spawn boss etc
        }
    }
}
