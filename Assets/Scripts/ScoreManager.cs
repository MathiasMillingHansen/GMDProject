using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameScene")
        {
            ResetScore();
            StartCoroutine(DecreaseScoreOverTime());
        }
        else
        {
            StopAllCoroutines(); // Stop the coroutine if leaving the GameScene
        }
    }

    private void ResetScore()
    {
        score = 20000;
        Debug.Log("Score reset to: " + score);
    }

    private IEnumerator DecreaseScoreOverTime()
    {
        while (SceneManager.GetActiveScene().name == "GameScene")
        {
            if (score > 0)
            {
                score -= 10;
                Debug.Log("Score: " + score);
            }
            yield return new WaitForSeconds(0.5f); // Wait for 200 milliseconds
        }
    }

    public void AddScore(int points)
    {
        score += points;
        Debug.Log("Current score: " + score);
    }

    public int GetScore()
    {
        return score;
    }
}