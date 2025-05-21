using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; 

    [Header("Audio Settings")]
    public AudioSource musicSource; 

    [Header("Audio Clips")]
    public AudioClip[] musicTracks;    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    void Start()
    {
        musicSource.loop = true;
        musicSource.volume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        PlayMusic(0);
    }

    public void PlayMusic(int trackIndex)
    {
        if (trackIndex < musicTracks.Length)
        {
            musicSource.clip = musicTracks[trackIndex];
            musicSource.Play();
        }
    }


    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
    }

    
}
