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

        [SerializeField] private EnemyData m_data;

        public EnemyData Data => m_data;

        public IObjectPool<Enemy> Pool
        {
            get => _pool;
            set => _pool = value;
        }


        private int _pathIndex = 0;
        private Path currentPath;
        private Vector3 _targetPosition;
        private IObjectPool<Enemy> _pool;
        private Enemy_Health _health;


        void Awake()
        {
            currentPath = GameObject.Find("Path1").GetComponent<Path>();
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
                m_data.moveSpeed * Time.deltaTime);

            float distance = (transform.position - _targetPosition).magnitude;
            if (distance < 0.1f)
            {
                if (_pathIndex < currentPath.wayPoints.Length - 1)
                {
                    _pathIndex++;
                    _targetPosition = currentPath.GetPointPosition(_pathIndex);
                }
                else // Reached the end of the path
                {
                    OnEnemyReachedEnd?.Invoke(m_data);
                    Deactive();
                }

            }
        }

        public void Deactive() => _pool.Release(this);

        private void Init()
        {
            _pathIndex = 0;
            _targetPosition = currentPath.GetPointPosition(_pathIndex);

            _health.Initialize(m_data);
        }
        private void HandleEnemyDie()
        {
            OnGetEnemyReward?.Invoke(m_data);
            GameEvent.HandleEnemyDie(this);

            Deactive();
        }

    }

}