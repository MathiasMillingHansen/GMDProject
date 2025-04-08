using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;  // Make sure to include this namespace

public class GameController : MonoBehaviour
{
    private GameControls controls;

    void Awake()
    {
        controls = new GameControls();
    }

    void OnEnable()
    {
        controls.UI.Enable();  // Enable the UI input actions
    }

    void OnDisable()
    {
        controls.UI.Disable();  // Disable the UI input actions
    }

    void Update()
    {
        // Check if Escape key or Start button on Xbox controller is pressed
        if (controls.UI.Cancel.triggered)
        {
            GoToMainMenu();
        }
    }

    void GoToMainMenu()
    {
        // Load the Main Menu scene
        SceneManager.LoadScene("MainMenu");
    }
}
