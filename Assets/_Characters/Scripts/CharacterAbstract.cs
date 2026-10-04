

using UnityEngine;

namespace Characters
{
    public interface ICterAbstract
    {
        CharacterSO SO { get; set; }
        Transform AttackPoint { get; }

        void SetTarget(Transform target);
        void Flip();
    }

    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CharacterColor))]
    public abstract class CharacterAbstract : MonoBehaviour,
    ICterAbstract
    {
        [SerializeField] protected CharacterSO Data;

        public CharacterSO SO { get; set; }

        public Transform AttackPoint { get; private set; }

        public Rigidbody2D Rb { get; private set; }
        public Animator Anim { get; private set; }

        protected virtual void Awake()
        {
            Anim = GetComponent<Animator>();
            Rb = GetComponent<Rigidbody2D>();

            AttackPoint = transform.Find("attack_point");
            SO = Data;
        }

        public Transform Target { get; protected set; }

        public void SetTarget(Transform target) => Target = target;

        public void Flip()
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
}