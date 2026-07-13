using UnityEngine;

namespace TDGame
{
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Animator _animator;
        private float _moveSpeed;
        private float _maxMoveSpeed;

        public void Init(float moveSpeed, float maxMoveSpeed)
        {
            _maxMoveSpeed = maxMoveSpeed;
            _moveSpeed = Mathf.Clamp(moveSpeed, 0, _maxMoveSpeed); ;
        }

        public void Move(Vector2 direction)
        {
            direction = direction.normalized;
            _rigidbody.linearVelocity = direction * _moveSpeed;
            _animator.SetFloat("moveX", direction.x);
            _animator.SetFloat("moveY", direction.y);
            _animator.SetFloat("Speed", _rigidbody.linearVelocity.magnitude);
        }

        public void Stop()
        {
            _rigidbody.linearVelocity = Vector2.zero;
            _animator.SetFloat("Speed", _rigidbody.linearVelocity.magnitude);
        }
    }
}