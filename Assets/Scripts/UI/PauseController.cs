using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu; // Assign this in the Inspector

    private bool isPaused = false;

    void Start()
    {
        if (pauseMenu != null)
            pauseMenu.SetActive(false);
    }

    void Update()
    {
        // Press "Escape" to toggle pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
        
        // Enable/Disable pause menu
        if (pauseMenu != null)
            pauseMenu.SetActive(isPaused);
    }

    public void RestartGame()
    {
        Time.timeScale = 1; // Ensure time resumes before restarting
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1; // Ensure time resumes before restarting
        SceneManager.LoadScene(0);
    }
}
