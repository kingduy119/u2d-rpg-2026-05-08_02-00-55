

using UnityEngine;

namespace Characters
{
    public interface ICterAbstract
    {
        Transform GetAttackPoint();
        CharacterSO GetData();

        void SetTarget(Transform target);
        // Vector2 Get

        void SetData(CharacterSO data);
        void Flip();
    }

    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class CharacterAbstract : MonoBehaviour,
    ICterAbstract
    {
        [SerializeField] protected CharacterSO Data;
        public CharacterSO GetData() => Data;
        public void SetData(CharacterSO data) => Data = data;

        public Transform AttackPoint;
        public Transform GetAttackPoint() => AttackPoint;

        public Rigidbody2D Rb { get; private set; }
        public Animator Anim { get; private set; }

        protected virtual void Awake()
        {
            Anim = GetComponent<Animator>();
            Rb = GetComponent<Rigidbody2D>();
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