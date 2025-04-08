using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsController : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;
    public Toggle fullscreenToggle;

    private const string MUSIC_VOLUME_KEY = "MusicVolume";
    private const string SFX_VOLUME_KEY = "SFXVolume";
    private const string FULLSCREEN_KEY = "Fullscreen";

    void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 1f);
        sfxSlider.value = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 1f); 
        fullscreenToggle.isOn = PlayerPrefs.GetInt(FULLSCREEN_KEY, 1) == 1; 

        musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        fullscreenToggle.onValueChanged.AddListener(OnFullscreenToggled);

        AudioManager.Instance.PlayMusic(1);
    }

    void OnMusicVolumeChanged(float volume)
    {
        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, volume);
        PlayerPrefs.Save();

        AudioListener.volume = volume; 
    }

    void OnSFXVolumeChanged(float volume)
    {
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, volume);
        PlayerPrefs.Save();
    }

    void OnFullscreenToggled(bool isFullscreen)
    {
        PlayerPrefs.SetInt(FULLSCREEN_KEY, isFullscreen ? 1 : 0);
        PlayerPrefs.Save();

        Screen.fullScreen = isFullscreen;
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
