using UnityEngine;

namespace TDGame
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;

        [Header("Music")]
        [SerializeField] private AudioClip mainMenuMusic;
        [SerializeField] private AudioClip gameplayMusic;

        [Header("Game SFX")]
        [SerializeField] private AudioClip pasue;
        [SerializeField] private AudioClip resume;
        [SerializeField] private AudioClip buttonClick;
        [SerializeField] private AudioClip missionComplete;
        [SerializeField] private AudioClip gameOver;

        [Header("Enemy SFX")]
        [SerializeField] private AudioClip enemyDestroyed;
        // [SerializeField] private AudioClip enemyDestroyed;

        [Header("Tower SFX")]
        [SerializeField] private AudioClip towerCantBuild;
        [SerializeField] private AudioClip towerPlaced;
        [SerializeField] private AudioClip towerAttack;

        protected override void Awake()
        {
            base.Awake();
        }

        public void PlayMusic(AudioClip clip)
        {
            if (musicSource.clip == clip && musicSource.isPlaying) return;
            musicSource.clip = clip;
            musicSource.loop = true;
            musicSource.Play();
        }

        public void PlaySoundEffect(AudioClip clip)
        {
            sfxSource.PlayOneShot(clip);
        }

        public void PlayMainMenuMusic() => PlayMusic(mainMenuMusic);
        public void PlayGameplayMusic() => PlayMusic(gameplayMusic);

        public void PlayPauseSound() => PlaySoundEffect(pasue);
        public void PlayResumeSound() => PlaySoundEffect(resume);
        public void PlayButtonClickSound() => PlaySoundEffect(buttonClick);
        public void PlayMissionCompleteSound() => PlaySoundEffect(missionComplete);
        public void PlayGameOverSound() => PlaySoundEffect(gameOver);

        public void PlayEnemyDestroyedSound() => PlaySoundEffect(enemyDestroyed);
        public void PlayEnemyTakeDamage() => PlaySoundEffect(enemyDestroyed);

        public void PlayTowerPlacedSound() => PlaySoundEffect(towerPlaced);
        public void PlayTowerCantBuild() => PlaySoundEffect(towerCantBuild);
        public void PlayTowerAttack() => PlaySoundEffect(towerAttack);

    }

}