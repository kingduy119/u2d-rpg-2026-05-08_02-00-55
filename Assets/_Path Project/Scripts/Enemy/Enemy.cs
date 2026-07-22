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
        public EnemySO Data;
        public EnemySound Sound;

        #region Private Fields
        private EnemyHealth _health;
        private CharacterMovement _movement;
        private Path PathWay => SpawnManager.Instance.MapPath;
        private Vector3 _targetPosition;
        private int _pathIndex = 0;
        #endregion

        public IObjectPool<Enemy> Pool { get; set; }

        private void Awake()
        {
            _health = GetComponent<EnemyHealth>();
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
                // Next waypoint or end
                if (_pathIndex < PathWay.wayPoints.Length - 1)
                {
                    _pathIndex++;
                    _targetPosition = PathWay.GetPointPosition(_pathIndex);
                }
                else
                {
                    GameEvent.SendEnemyReachedEnd(this);
                    Deactive();
                }
            }
        }

        private void Reset()
        {
            _health.Init(this);
            _movement.Init(Data.moveSpeed, Data.moveSpeed + 3);
            _pathIndex = 0;
            _targetPosition = PathWay.GetPointPosition(_pathIndex);
        }

        public void Deactive()
        {
            Reset();
            Pool.Release(this);
        }

    }

}