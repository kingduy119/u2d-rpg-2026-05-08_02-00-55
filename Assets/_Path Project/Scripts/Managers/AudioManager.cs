using UnityEngine;

public class AudioManager : PersistentSingleton<AudioManager>
{
    // public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    public AudioClip mainMenuMusic;
    public AudioClip gameplayMusic;

    public AudioClip pasueClip;
    public AudioClip resumeClip;
    public AudioClip buttonClickClip;

    public AudioClip towerPlacedClip;
    public AudioClip enemyDestroyedClip;
    public AudioClip missionCompleteClip;
    public AudioClip gameOverClip;

    // private void Awake()
    // {
    //     if (Instance != null && Instance != this)
    //     {
    //         Destroy(gameObject);
    //     }
    //     else
    //     {
    //         Instance = this;
    //         DontDestroyOnLoad(gameObject);
    //     }
    // }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip == clip && musicSource.isPlaying) return;
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySound(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayPauseSound() => PlaySound(pasueClip);
    public void PlayResumeSound() => PlaySound(resumeClip);
    public void PlayButtonClickSound() => PlaySound(buttonClickClip);
    public void PlayTowerPlacedSound() => PlaySound(towerPlacedClip);
    public void PlayEnemyDestroyedSound() => PlaySound(enemyDestroyedClip);
    public void PlayMissionCompleteSound() => PlaySound(missionCompleteClip);
    public void PlayGameOverSound() => PlaySound(gameOverClip);

}
