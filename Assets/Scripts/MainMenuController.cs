using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuControlller : MonoBehaviour
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

    public void GoToSettings(){
        SceneManager.LoadScene("SettingsScene");
    }
}
