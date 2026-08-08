using System;
using UnityEngine;
using UnityEngine.Pool;

namespace TDGame
{
    [RequireComponent(typeof(EnemyHealth))]
    [RequireComponent(typeof(CharacterMovement))]
    public class Enemy : MonoBehaviour,
        IPoolable<Enemy>
    {
        public EnemySO SO;
        public EnemySound Sound;


        private int _pathIndex = 0;
        private CharacterMovement _movement;
        private Vector3 _targetPosition;
        private GameObject[] _Pathway;

        public IObjectPool<Enemy> Pool { get; set; }

        private void Awake()
        {
            _movement = GetComponent<CharacterMovement>();
        }

        private void Start()
        {
            Reset();
        }

        private void FixedUpdate()
        {
            Vector2 direction = (_targetPosition - transform.position).normalized;
            _movement.Move(direction);

            float distance = Vector2.Distance(transform.position, _targetPosition);
            if (distance < 0.05f)
            {
                if (_pathIndex < _Pathway.Length - 1)
                {
                    _pathIndex++;
                    _targetPosition = _Pathway[_pathIndex].transform.position;
                }
                else
                {
                    // GameEvent.SendEnemyReachedEnd(this);
                    EnemyEvent.OnEnemyReachedEnd?.Invoke(this);
                    Deactivate();
                }
            }
        }

        private void Reset()
        {
            _movement.Init(SO.moveSpeed, SO.moveSpeed + 3);
            _pathIndex = 0;
            _targetPosition = _Pathway[_pathIndex].transform.position;
        }

        public void SetPathway(GameObject[] pathway)
        {
            _Pathway = pathway;
        }

        public void Deactivate()
        {
            Reset();
            Pool.Release(this);
        }

    }

}