
using System;
using UnityEngine;

namespace TDGame
{
    [Serializable]
    public class EnemySound
    {
        [SerializeField] public AudioClip TakeDamgeSFX;
        [SerializeField] public AudioClip DestroySFX;

        public void PlayTakeDamage() => AudioEvent.OnPlaySFX?.Invoke(TakeDamgeSFX);
        public void PlayDestroy() => AudioEvent.OnPlaySFX?.Invoke(DestroySFX);
    }
}