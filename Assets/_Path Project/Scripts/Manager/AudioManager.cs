using UnityEngine;

public class AudioManager : PersistentSingleton<AudioManager>
{

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Music")]
    [SerializeField] private AudioClip mainMenuMusic;
    [SerializeField] private AudioClip gameplayMusic;

    [Header("Button SFX")]
    [SerializeField] private AudioClip pasueClip;
    [SerializeField] private AudioClip resumeClip;
    [SerializeField] private AudioClip buttonClickClip;

    [Header("Object SFX")]
    [SerializeField] private AudioClip towerPlacedClip;
    [SerializeField] private AudioClip enemyDestroyedClip;
    [SerializeField] private AudioClip missionCompleteClip;
    [SerializeField] private AudioClip gameOverClip;

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

    public void PlayMainMenuMusic() => PlayMusic(mainMenuMusic);
    public void PlayGameplayMusic() => PlayMusic(gameplayMusic);

    public void PlayPauseSound() => PlaySound(pasueClip);
    public void PlayResumeSound() => PlaySound(resumeClip);
    public void PlayButtonClickSound() => PlaySound(buttonClickClip);
    public void PlayTowerPlacedSound() => PlaySound(towerPlacedClip);
    public void PlayEnemyDestroyedSound() => PlaySound(enemyDestroyedClip);
    public void PlayMissionCompleteSound() => PlaySound(missionCompleteClip);
    public void PlayGameOverSound() => PlaySound(gameOverClip);

}
