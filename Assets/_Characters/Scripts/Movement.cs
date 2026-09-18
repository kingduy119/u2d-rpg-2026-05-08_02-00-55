

using UnityEngine;

namespace Characters
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Animator _animator;

        private MoveData Data;
        public void SetData(CharacterSO SO) => Data = SO.Move;

        public void Move(Vector2 input)
        {
            Vector2 direction = input.normalized;
            _rigidbody.linearVelocity = direction * Data.Speed;

            _animator.SetFloat("moveX", Mathf.Abs(direction.x));
            _animator.SetFloat("moveY", Mathf.Abs(direction.y));
            _animator.SetFloat("Speed", _rigidbody.linearVelocity.magnitude);

            if (direction.x > 0 && transform.localScale.x < 0 ||
                direction.x < 0 && transform.localScale.x > 0)
            {
                Flip();
            }
        }

        public void Stop()
        {
            _rigidbody.linearVelocity = Vector2.zero;
            _animator.SetFloat("Speed", _rigidbody.linearVelocity.magnitude);
        }

        private void Flip()
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
}