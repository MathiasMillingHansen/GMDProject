using UnityEngine;
using UnityEngine.SceneManagement;

public class DiedScreenControlller : MonoBehaviour
{

    public void Start()
    {
        AudioManager.Instance.PlayMusic(0);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GoToMainMenu(){
        SceneManager.LoadScene("MainMenu");
    }
}
