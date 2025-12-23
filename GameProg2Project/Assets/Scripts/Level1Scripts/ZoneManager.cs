using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ZoneManager : MonoBehaviour
{
    [System.Serializable]
    public class DoorUnlock
    {
        public int killsRequired;
        public GameObject doorToBreak;
    }

    public DoorUnlock[] doorUnlocks;
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public int totalEnemiesToSpawn = 20;
    public int maxEnemies = 5; 
    public float spawnDelay = 3f; 

    private int currentKills = 0;
    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private int nextUnlockIndex = 0;
    private int spawnedCount = 0;
    private float nextSpawnTime = 0f;
    private bool spawningComplete = false;

    public Text killCounterText;
    public Text enemiesAliveText;
    public Text nextDoorText;

    void Start()
    {
        nextSpawnTime = Time.time;
        if (killCounterText == null)
            killCounterText = GameObject.Find("KillCount").GetComponent<Text>();
        if (enemiesAliveText == null)
            enemiesAliveText = GameObject.Find("EnemiesAlive").GetComponent<Text>();
        if (nextDoorText == null)
            nextDoorText = GameObject.Find("NextDoor").GetComponent<Text>();

        nextSpawnTime = Time.time;
        UpdateUI();
    }

    void Update()
    {
        if (!spawningComplete && Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
        }
        CheckForDeadEnemies();
        UpdateUI();
    }

    void SpawnEnemy()
    {
        if (spawnedCount >= totalEnemiesToSpawn)
        {
            spawningComplete = true;
            return;
        }

        if (spawnedEnemies.Count >= maxEnemies)
        {
            nextSpawnTime = Time.time + 0.5f;
            return;
        }

        Transform spawnPoint = GetRandomSpawnPoint();
        if (spawnPoint != null)
        {
            SpawnEnemy(spawnPoint);
            nextSpawnTime = Time.time + spawnDelay;
        }
        else
        {
            Debug.LogWarning("No valid spawn points available!");
            nextSpawnTime = Time.time + 1f;
        }
    }

    void SpawnEnemy(Transform spawnPoint)
    {
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        spawnedEnemies.Add(enemy);
        spawnedCount++;

        Debug.Log($"Spawned enemy {spawnedCount}/{totalEnemiesToSpawn} at {spawnPoint.name}");
    }

    Transform GetRandomSpawnPoint()
    {
        if (spawnPoints.Length == 0)
            return null;

        List<Transform> availablePoints = new List<Transform>(spawnPoints);

        for (int i = 0; i < availablePoints.Count; i++)
        {
            Transform temp = availablePoints[i];
            int randomIndex = Random.Range(i, availablePoints.Count);
            availablePoints[i] = availablePoints[randomIndex];
            availablePoints[randomIndex] = temp;
        }

        return availablePoints[Random.Range(0, availablePoints.Count)];
    }

    void CheckForDeadEnemies()
    {
        for (int i = spawnedEnemies.Count - 1; i >= 0; i--)
        {
            if (spawnedEnemies[i] == null)
            {
                spawnedEnemies.RemoveAt(i);
                OnEnemyKilled();
            }
        }
    }

    void OnEnemyKilled()
    {
        currentKills++;
        Debug.Log($"Kill {currentKills}/{totalEnemiesToSpawn}");

        CheckDoorUnlocks();

        nextSpawnTime = Time.time + (spawnDelay * 0.5f);
    }

    void CheckDoorUnlocks()
    {
        while (nextUnlockIndex < doorUnlocks.Length &&
               currentKills >= doorUnlocks[nextUnlockIndex].killsRequired)
        {
            UnlockDoor(doorUnlocks[nextUnlockIndex].doorToBreak);
            nextUnlockIndex++;
        }
    }

    void UnlockDoor(GameObject door)
    {
        if (door != null)
        {
            door.SetActive(false);
            Debug.Log($"unlocked at {currentKills}");
        }
    }

    public void UpdateUI()
    {
        if (killCounterText != null)
            killCounterText.text = $"Kills: {currentKills}/{totalEnemiesToSpawn}";

        // Update enemies alive
        if (enemiesAliveText != null)
            enemiesAliveText.text = $"Enemies alive: {spawnedEnemies.Count}";

        if (nextDoorText != null)
        {
            if (nextUnlockIndex < doorUnlocks.Length)
            {
                nextDoorText.text = $"Next door at: {doorUnlocks[nextUnlockIndex].killsRequired} kills";
                nextDoorText.gameObject.SetActive(true);
            }
            else
            {
                nextDoorText.gameObject.SetActive(false);
            }
        }
    }
}