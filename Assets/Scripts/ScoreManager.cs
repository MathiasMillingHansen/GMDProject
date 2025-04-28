using UnityEngine;
using UnityEngine.SceneManagement; // Required to access scene information

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    private int score = 20000;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep the ScoreManager across scenes.
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // Check if the active scene is "GameScene"
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            // Ensure the score doesn't go below 0
            if (score > 0)
            {
                score -= 10;
            }
        }
    }

    // Method to add points to the score
    public void AddScore(int points)
    {
        score += points;
        Debug.Log("Current score: " + score); // Log the current score
    }
}