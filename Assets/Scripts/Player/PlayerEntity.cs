using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerEntity : MonoBehaviour
    {
        [Header("HorizontalMovement")]
        [SerializeField] private float _horizontalSpeed;
        [SerializeField] private bool _faceRight;

        [Header("Jump")]
        [SerializeField] private float _jumpForce;

        private Rigidbody2D _rigidbody;
        
        private bool _isJumping;
        private float _startJumpVerticalPosition;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (_isJumping)
                UpdateJump();
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
            SetDirection(direction);
            var velocity = _rigidbody.velocity;
            velocity.x = _horizontalSpeed * direction;
            _rigidbody.velocity = velocity;
        }
        
        private void SetDirection(float direction)
        {
            if(_faceRight && direction < 0 || 
               !_faceRight && direction > 0)
                Flip();
        }

        private void Flip()
        {
            transform.Rotate(0, 180, 0);
            _faceRight = !_faceRight;
        }
    }
}