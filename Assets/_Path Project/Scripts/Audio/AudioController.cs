
using UnityEngine;

namespace TDGame
{
    public class AudioController : MonoBehaviour
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

        private void OnEnable()
        {
            AudioEvent.OnPlaySFX += PlaySoundEffect;
        }

        private void OnDisable()
        {
            AudioEvent.OnPlaySFX -= PlaySoundEffect;
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
    }
}