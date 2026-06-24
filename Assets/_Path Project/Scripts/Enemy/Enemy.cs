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


        Enemy_Health _health;

        private Path currentPath;
        private Vector3 _targetPosition;
        private int _pathIndex = 0;
        private IObjectPool<Enemy> _pool;


        void Awake()
        {
            currentPath = GameObject.Find("Path1").GetComponent<Path>();
            _health = GetComponent<Enemy_Health>();
        }

        private void OnEnable()
        {
            _health.OnEnemyDie += HandleEnemyDie;

        }
        private void OnDisable()
        {
            _health.OnEnemyDie -= HandleEnemyDie;
        }

        private void Start()
        {
            _pathIndex = 0;
            _targetPosition = currentPath.GetPointPosition(_pathIndex);
            _health.Initialize(_data);
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
                if (_pathIndex < currentPath.wayPoints.Length - 1)
                {
                    _pathIndex++;
                    _targetPosition = currentPath.GetPointPosition(_pathIndex);
                }
                else // Reached the end of the path
                {
                    OnEnemyReachedEnd?.Invoke(_data);
                    Deactive();
                }

            }
        }

        private void HandleEnemyDie()
        {
            OnGetEnemyReward?.Invoke(_data);
            GameEvent.HandleEnemyDie(this);

            Deactive();
        }
        public void Deactive() => _pool.Release(this);
    }

}