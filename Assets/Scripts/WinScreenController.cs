using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Required for working with UI elements

public class WinScreenControlller : MonoBehaviour
{
    public Text ScoreText; // Reference to the UI Text element

    private void Start()
    {
        // Play background music
        AudioManager.Instance.PlayMusic(0);

        // Get the score from the ScoreManager and display it
        if (ScoreManager.Instance != null)
        {
            ScoreText.text = "Score: " + ScoreManager.Instance.GetScore();
        }
        else
        {
            ScoreText.text = "Score: 0"; // Fallback if ScoreManager is not found
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}