using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems; 

public class GameController : MonoBehaviour
{
    private GameControls controls;
    public GameObject pauseMenu; // Assign this in the Inspector
    public GameObject resumeButton; // Assign the Resume button in the Inspector

    public static bool isPaused = false;

    void Awake()
    {
        controls = new GameControls();
    }

    void Start()
    {
        isPaused = false; // Reset the paused state
        Time.timeScale = 1f; // Ensure the game runs at normal speed
    }

    void OnEnable()
    {
        controls.UI.Enable();
    }

    void OnDisable()
    {
        controls.UI.Disable();
    }

    void Update()
    {
        // Check if Escape key or Start button on Xbox controller is pressed
        if (controls.UI.Cancel.triggered)
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

    void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // Freeze the game
        pauseMenu.SetActive(true); // Show the pause menu

        // Set the default selected button
        EventSystem.current.SetSelectedGameObject(resumeButton);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Resume the game
        pauseMenu.SetActive(false); // Hide the pause menu
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // Ensure time scale is reset
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit(); // Quit the application
    }
}