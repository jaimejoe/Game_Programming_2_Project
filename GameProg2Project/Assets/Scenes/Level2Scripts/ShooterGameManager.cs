using UnityEngine;

public class ShooterGameManager : MonoBehaviour
{
    public static ShooterGameManager Instance;

    public int enemiesAlive = 0;
    public GameObject exitDoor;
    public int PlayerHealth = 100;

    void Awake()
    {
        Instance = this;
        
    }

    public void EnemieSpawned()
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
            exitDoor.active = true;
            // unlock door / load next level / spawn boss etc
        }
    }
    public void DamagePlayer(int dmg)
    {
        PlayerHealth -= dmg;
        if (PlayerHealth <= 0)
        {

        }
    }

}
