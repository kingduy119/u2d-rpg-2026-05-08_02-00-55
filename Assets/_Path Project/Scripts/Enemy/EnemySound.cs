
using System;
using UnityEngine;

namespace TDGame
{
    [Serializable]
    public class EnemySound
    {
        [SerializeField] public AudioClip TakeDamgeSFX;
        [SerializeField] public AudioClip DestroySFX;

        public void PlayTakeDamage() => GameEvent.PlaySFX(TakeDamgeSFX);
        public void PlayDestroy() => GameEvent.PlaySFX(DestroySFX);
    }
}