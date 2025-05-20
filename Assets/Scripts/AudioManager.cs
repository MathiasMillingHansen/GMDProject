using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; 

    [Header("Audio Settings")]
    public AudioSource musicSource; 
    public AudioSource sfxSource;   

    [Header("Audio Clips")]
    public AudioClip[] musicTracks;  
    public AudioClip[] sfxClips;     

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
        sfxSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1f);
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

    public void PlaySFX(int sfxIndex)
    {
        if (sfxIndex < sfxClips.Length)
        {
            sfxSource.PlayOneShot(sfxClips[sfxIndex]);
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();
    }

    
}
