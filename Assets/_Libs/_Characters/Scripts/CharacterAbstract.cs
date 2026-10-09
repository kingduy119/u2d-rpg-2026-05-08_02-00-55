

using UnityEngine;

namespace Characters
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class CharacterAbstract : MonoBehaviour,
    ICterAbstract
    {
        public CharacterSO DefaultSO;


        public Animator Anim { get; protected set; }
        public Rigidbody2D Rb { get; protected set; }

        public CharacterSO SO { get; set; }

        protected void OnValidate()
        {
            SO = DefaultSO;
        }

        protected virtual void Awake()
        {
            Anim = GetComponent<Animator>();
            Rb = GetComponent<Rigidbody2D>();
            SO = DefaultSO;
        }

        public void Flip()
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
}