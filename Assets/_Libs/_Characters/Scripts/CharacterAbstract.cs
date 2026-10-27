

using UnityEngine;
using Weapons;

namespace Characters
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody2D))]
    // [RequireComponent(typeof(CharacterCombat))]
    public abstract class CharacterAbstract : MonoBehaviour,
        ICterAbstract
    {
        public CharacterSO DefaultSO;

        public Animator Anim { get; protected set; }
        public Rigidbody2D Rb { get; protected set; }

        public CharacterSO SO { get; set; }

        // [SerializeField] protected CharacterCombat _combat;


        protected void OnValidate()
        {
            SO = DefaultSO;
        }

        protected virtual void Awake()
        {
            Anim = GetComponent<Animator>();
            Rb = GetComponent<Rigidbody2D>();
            SO = DefaultSO;

            // _combat = GetComponent<CharacterCombat>();
        }

        public void Flip()
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
}