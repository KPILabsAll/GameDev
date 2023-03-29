using Core.Enums;
using Player.PlayerAnimation;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerEntity : MonoBehaviour
    {
        [SerializeField] private AnimatorController _animator;
        
        [Header("HorizontalMovement")]
        [SerializeField] private float _horizontalSpeed;
        [SerializeField] private Direction _direction;

        [Header("Jump")]
        [SerializeField] private float _jumpForce;

        [SerializeField] private DirectionalCameraPair _cameras;

        private Rigidbody2D _rigidbody;
        
        private bool _isJumping;
        private float _startJumpVerticalPosition;
        
        private Vector2 _movement;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (_isJumping)
                UpdateJump();
            
            UpdateAnimations();
        }
        
        public void Jump()
        {
            if (_isJumping) return;
        
            _isJumping = true;
            _rigidbody.AddForce(_jumpForce * Vector2.up);
            _startJumpVerticalPosition = transform.position.y;
        }

        private void UpdateJump()
        {
            if (_rigidbody.velocity.y < 0 && _rigidbody.position.y <= _startJumpVerticalPosition)
            {
                ResetJump();
                return;
            }
        }
        
        private void ResetJump()
        {
            _isJumping = false;
            _rigidbody.position = new(_rigidbody.position.x, _startJumpVerticalPosition);
        }
        
        public void MoveHorizontally(float direction)
        {
            _movement.x = direction;
            SetDirection(direction);
            var velocity = _rigidbody.velocity;
            velocity.x = _horizontalSpeed * direction;
            _rigidbody.velocity = velocity;
        }
        
        private void SetDirection(float direction)
        {
            if(_direction == Direction.Right && direction < 0 || 
               _direction == Direction.Left && direction > 0)
                Flip();
        }

        private void Flip()
        {
            transform.Rotate(0, 180, 0);
            _direction = _direction == Direction.Right ? Direction.Left : Direction.Right;
            foreach (var cameraPair in _cameras.DirectionalCameras)
                cameraPair.Value.enabled = cameraPair.Key == _direction;
        }
        
        private void UpdateAnimations()
        {
            _animator.PlayAnimation(AnimationType.Idle, true);
            _animator.PlayAnimation(AnimationType.Walk, _movement.magnitude > 0);
            _animator.PlayAnimation(AnimationType.Jump, _isJumping);
        }
    }
}