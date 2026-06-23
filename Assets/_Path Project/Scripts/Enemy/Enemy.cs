using System;
using UnityEngine;

namespace TDGame
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private EnemyData _data;
        public EnemyData Data => _data;

        // [SerializeField] private Transform _healthBar;

        public static event Action<EnemyData> OnEnemyReachedEnd;
        public static event Action<Enemy> OnEnemyDestroyed;

        Enemy_Health m_health;

        private Path currentPath;
        private Vector3 _targetPosition;
        private int _pathIndex = 0;
        private float _lives;
        // private Vector3 _healthBarOriginalScale;

        void Awake()
        {
            currentPath = GameObject.Find("Path1").GetComponent<Path>();
            // _healthBarOriginalScale = _healthBar.localScale;
            m_health = GetComponent<Enemy_Health>();

            m_health.Initialize(_data);
        }

        void OnEnable()
        {
            _pathIndex = 0;
            _targetPosition = currentPath.GetPointPosition(_pathIndex);

            // _lives = _data.lives;
            // UpdateHealthBar();
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
                    gameObject.SetActive(false);
                }

            }
        }

        public void TakeDamge(TowerData data)
        {
            // _lives -= data.damage;
            // if (_lives <= 0)
            // {
            //     OnEnemyDestroyed?.Invoke(this);
            //     gameObject.SetActive(false);
            // }
            // UpdateHealthBar();
        }

        // private void UpdateHealthBar()
        // {
        //     float percent = _lives / _data.lives;
        //     Vector3 scale = _healthBarOriginalScale;
        //     scale.x = _healthBarOriginalScale.x * percent;
        //     _healthBar.localScale = scale;
        // }
    }

}