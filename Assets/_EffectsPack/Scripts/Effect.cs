using System;
using UnityEngine;
using UnityEngine.Pool;

namespace EffectPack
{
    [RequireComponent(typeof(Animator))]
    public class Effect : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        public IObjectPool<Effect> Pool { get; set; }

        void Start()
        {
            animator = GetComponent<Animator>();
        }

        protected void OnTriggerEnter2D(Collider2D collision)
        {
            PlaySound();
        }

        protected void PlaySound()
        {
            Debug.Log("Effect: Play Sound");
        }

        public virtual void Deactivate()
        {
            gameObject.SetActive(false);
        }
    }


    public static class EffectEvent
    {
        public static event Action<AudioClip> OnPlaySoundEffect;
        public static void SendPlaySoundEffect(AudioClip clip)
        {
            OnPlaySoundEffect?.Invoke(clip);
        }
    }
}