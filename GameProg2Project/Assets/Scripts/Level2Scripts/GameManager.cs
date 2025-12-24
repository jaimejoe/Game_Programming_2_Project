using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject EndScreenPanel;
    [SerializeField] private GameObject Hud;
    [SerializeField] private Text HudObjective;
    [SerializeField] private Text HudEnemyCount;
    //[SerializeField] private CharacterMovement player;
    //[SerializeField] private Image[] hearts;

    //public float timePassed;
    //public Text endGameScore;
    //public Text timerText;

    //[Header("Sprite Settings")]
    //[SerializeField] private Sprite EmptyHeart;
    //[SerializeField] private Sprite FullHeart;
    public int PlayerHp = 100;
    public int PlayerMaxHp = 100;
    public int CowHp = 600;
    public int CowHpMax = 600;
    public bool isPaused = false;
    public Slider HealthBar;
    public bool gameOver = false;

    public int totalEnemies = 0;
    public GameObject Doors;


    // =============================== Score Board ===================================

    [Header("Score Board Settings")]
    public Text text;
    public int score = 0;
    

    private void Awake()
    {
        //timePassed = 0f;
        //DontDestroyOnLoad(gameObject);
        HealthBar.value = PlayerHp;
        DontDestroyOnLoad(gameObject);
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }


    void Update()
    {
        
        if (PlayerHp <= 0)
        {
            GameOverScreen();
        }

        // Check if the player presses the "Escape" key (or any key you choose).
        if (Input.GetKeyDown(KeyCode.Escape) && !gameOver)
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    //Score Increase
    public void ScoreIncrease()
    {
        score += 50;
        text.text = "Score: "+ score;
    }
    //Game Over screen
    void GameOverScreen()
    {
        gameOver = true;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Hud.SetActive(false);
        pauseMenuPanel.SetActive(false);
        EndScreenPanel.SetActive(true);
    }

    //Health Decrease
    public void HealthDecrease(int dmg)
    {
        PlayerHp-=dmg;
        HealthBar.value = PlayerHp;
        if(PlayerHp <= 0)
        {
            GameOverScreen();
        }
    }

    // This method pauses the game.
    public void PauseGame()
    {
        Hud.SetActive(false);
        // Show Pause Menu UI
        pauseMenuPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        // Freeze game time
        Time.timeScale = 0f;
        isPaused = true;
    }
    public void EndGame()
    {
        // Show End Menu UI
        PauseGame();
        Hud.SetActive(false);
        pauseMenuPanel.SetActive(false);
        EndScreenPanel.SetActive(true);
        //endGameScore.text = ""+score;
        //timerText.text = timePassed.ToString();
        // Freeze game time
        Time.timeScale = 0f;
        
    }
    public void RestartGame()
    {
        ResetHealth();
        EndScreenPanel.SetActive(false);
        SceneManager.LoadScene("Level1");
        ResumeGame();
    }

    // This method resumes the game.
    public void ResumeGame()
    {
        //Debug.Log("Resume button pressed!");
        // Hide Pause Menu UI
        pauseMenuPanel.SetActive(false);
        Hud.SetActive(true );
        // Unfreeze game time
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        // (Optional) Unfreeze audio
        // AudioListener.pause = false;
        isPaused = false;
    }
    public void NextLevel()
    {
        ResetHealth();

        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextScene);
        if(nextScene == 4)
        {
            EndGame();
        }

        Debug.Log("Next Level");
        ResumeGame();

    }
    public void OffMap()
    {
        HealthDecrease(10);
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
    public void GameOver()
    {
        Time.timeScale = 0f;
        gameOver = true;
        Hud.SetActive(false);
        EndScreenPanel.SetActive(true);
        
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;

        // Reload the currently active scene
        ResetHealth();
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);

        Debug.Log("Level restarted!");
        ResumeGame();
    }
    public void ResetHealth()
    {
        PlayerHp = PlayerMaxHp;
    }

    // Optional: a method for quitting the game or returning to the main menu.
    public void QuitGame()
    {
        // If you're in the editor, this won't fully work,
        // but in a built application, this will quit the game.
        Debug.Log("Quit button pressed!");
        Application.Quit();
        // If you have a Main Menu scene, you might do:
        // SceneManager.LoadScene("MainMenu");
    }
    public void SpawnedEnemy()
    {
        HudObjective.text = "Objective: Kill all Cowboys!!!";
        totalEnemies++;
        //HudEnemyCount.text = "Enemies Remaining: "+totalEnemies;
    }
    public void DeadEnemy()
    {

        totalEnemies--;
        if(totalEnemies <= 0)
        {
            UnlockDoors();
            HudObjective.text = "Objective Go to the Big Yellow Door";
        }
        HudEnemyCount.text = "Enemies Remaining: " + totalEnemies;
    }
    private void UnlockDoors()
    {
        Doors.SetActive(true);
    }
    


}
