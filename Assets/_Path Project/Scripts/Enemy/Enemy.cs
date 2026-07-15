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
        [SerializeField] private EnemyData _data;
        public IObjectPool<Enemy> Pool { get; set; }

        #region Private Fields
        private EnemyHealth _health;
        private CharacterMovement _movement;
        private Path _currentPath => SpawnManager.Instance.MapPath;
        private Vector3 _targetPosition;
        private int _pathIndex = 0;
        #endregion

        public EnemyData Data => _data;

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
                if (_pathIndex < _currentPath.wayPoints.Length - 1)
                {
                    _pathIndex++;
                    _targetPosition = _currentPath.GetPointPosition(_pathIndex);
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
            _movement.Init(_data.moveSpeed, _data.moveSpeed + 3);
            _pathIndex = 0;
            _targetPosition = _currentPath.GetPointPosition(_pathIndex);
        }

        public void Deactive()
        {
            Reset();
            Pool.Release(this);
        }

    }

}