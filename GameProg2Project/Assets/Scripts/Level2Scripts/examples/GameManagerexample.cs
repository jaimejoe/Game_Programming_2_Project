using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject EndScreenPanel;
    [SerializeField] private GameObject Hud;
    [SerializeField] private CharacterMovement player;
    [SerializeField] private Image[] hearts;

    public float timePassed;
    public Text endGameScore;
    public Text timerText;

    [Header("Sprite Settings")]
    [SerializeField] private Sprite EmptyHeart;
    [SerializeField] private Sprite FullHeart;
    public int health;
    private bool isPaused = false;

    // =============================== Score Board ===================================

    [Header("Score Board Settings")]
    public Text text;
    public int score = 0;
    

    private void Awake()
    {
        timePassed = 0f;
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(gameObject);
        health = 3;
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }


    void Update()
    {
        timePassed += Time.deltaTime; //timer
        
        if (health <= 0)
        {
            RestartLevel();
        }

        // Check if the player presses the "Escape" key (or any key you choose).
        if (Input.GetKeyDown(KeyCode.Escape))
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

    //Health Decrease
    public void HealthDecrease()
    {
        health --;
        hearts[health].sprite = EmptyHeart;
        if(health <= 0)
        {
            RestartLevel();
        }
    }

    // This method pauses the game.
    public void PauseGame()
    {
        // Show Pause Menu UI
        pauseMenuPanel.SetActive(true);
        // Freeze game time
        Time.timeScale = 0f;
        isPaused = true;
    }
    public void EndGame()
    {
        // Show End Menu UI
        PauseGame();
        Hud.SetActive(false);
        EndScreenPanel.SetActive(true);
        endGameScore.text = ""+score;
        timerText.text = timePassed.ToString();
        // Freeze game time
        Time.timeScale = 0f;
        
    }
    public void RestartGame()
    {
        ResetHealth();
        EndScreenPanel.SetActive(false);
        SceneManager.LoadScene("Tutorial");
        ResumeGame();
    }

    // This method resumes the game.
    public void ResumeGame()
    {
        //Debug.Log("Resume button pressed!");
        // Hide Pause Menu UI
        pauseMenuPanel.SetActive(false);
        // Unfreeze game time
        Time.timeScale = 1f;
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
        HealthDecrease();
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
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
        hearts[1].sprite = FullHeart;
        hearts[2].sprite = FullHeart;
        hearts[0].sprite = FullHeart;
        health = 3;
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
}
