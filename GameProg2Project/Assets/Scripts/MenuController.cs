using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void ToControlsMenu()
    {
        SceneManager.LoadScene("ControlsMenu");
    }
    public void ToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
