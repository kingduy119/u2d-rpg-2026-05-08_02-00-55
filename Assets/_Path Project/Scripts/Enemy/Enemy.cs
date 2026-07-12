using System;
using UnityEngine;
using UnityEngine.Pool;

namespace TDGame
{
    [RequireComponent(typeof(Enemy_Health))]
    public class Enemy : MonoBehaviour
    {
        public static event Action<EnemyData> OnEnemyReachedEnd;
        public static event Action<EnemyData> OnGetEnemyReward;

        [SerializeField] private EnemyData _data;

        public EnemyData Data => _data;

        public IObjectPool<Enemy> Pool
        {
            get => _pool;
            set => _pool = value;
        }

        #region Private Fields
        private int _pathIndex = 0;
        private Path _currentPath;
        private Vector3 _targetPosition;
        private IObjectPool<Enemy> _pool;
        private Enemy_Health _health;
        #endregion


        void Awake()
        {
            _currentPath = GameObject.Find("Path1").GetComponent<Path>();
            _health = GetComponent<Enemy_Health>();
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

        void Update()
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                _targetPosition,
                _data.moveSpeed * Time.deltaTime);

            float distance = (transform.position - _targetPosition).magnitude;
            if (distance < 0.1f)
            {
                if (_pathIndex < _currentPath.wayPoints.Length - 1)
                {
                    _pathIndex++;
                    _targetPosition = _currentPath.GetPointPosition(_pathIndex);
                }
                else // Reached the end of the path
                {
                    OnEnemyReachedEnd?.Invoke(_data);
                    Deactive();
                }
            }
        }

        public void Deactive() => _pool.Release(this);

        private void Init()
        {
            _pathIndex = 0;
            _targetPosition = _currentPath.GetPointPosition(_pathIndex);

            _health.Initialize(_data);
        }
        private void HandleEnemyDie()
        {
            OnGetEnemyReward?.Invoke(_data);
            GameEvent.HandleEnemyDie(this);

            Deactive();
        }

    }

}