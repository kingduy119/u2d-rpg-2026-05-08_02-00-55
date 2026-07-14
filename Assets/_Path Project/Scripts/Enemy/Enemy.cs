using System;
using UnityEngine;
using UnityEngine.Pool;

namespace TDGame
{
    [RequireComponent(typeof(Enemy_Health))]
    [RequireComponent(typeof(CharacterMovement))]
    public class Enemy : MonoBehaviour,
        IPoolable<Enemy>
    {
        [SerializeField] private EnemyData _data;
        public IObjectPool<Enemy> Pool { get; set; }

        #region Private Fields
        private Enemy_Health _health;
        private CharacterMovement _movement;
        private Path _currentPath => SpawnManager.Instance.MapPath;
        private Vector3 _targetPosition;
        private int _pathIndex = 0;
        #endregion


        void Awake()
        {
            _health = GetComponent<Enemy_Health>();
            _movement = GetComponent<CharacterMovement>();
        }

        private void OnEnable()
        {
            _health.OnEnemyDie += HandleEnemyDie;
            Init();
        }
        private void OnDisable()
        {
            _health.OnEnemyDie -= HandleEnemyDie;
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
                    GameEvent.SendEnemyReachedEnd(_data);
                    Deactive();
                }
            }
        }

        private void Init()
        {
            _pathIndex = 0;
            _targetPosition = _currentPath.GetPointPosition(_pathIndex);
            _health.Initialize(_data);
            _movement.Init(_data.moveSpeed, _data.moveSpeed + 3);
        }

        private void HandleEnemyDie()
        {
            GameEvent.SendEnemyReward(_data);
            GameEvent.HandleEnemyDie(this);

            Deactive();
        }

        public void Deactive() => Pool.Release(this);

    }

}