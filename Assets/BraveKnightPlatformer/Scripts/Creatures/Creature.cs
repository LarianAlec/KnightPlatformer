using KnightPlatformer.Components;
using KnightPlatformer.Utils;
using UnityEditor;
using UnityEngine;

namespace KnightPlatformer.Creatures
{
    public class Creature : MonoBehaviour
    {
        [Space]
        [Header("Properties")]
        [SerializeField] private float _speed = 5.0f;
        [SerializeField] protected float _jumpSpeed = 10f;
        [SerializeField] private float _damageVelocity = 5f;
        [SerializeField] private int _damage = 1;

        [Space]
        [Header("Checkers")]
        [SerializeField] protected LayerMask _groundLayer;
        [SerializeField] private LayerCheck _groundCheck;

        [SerializeField] private CheckCircleOverlap _attackRange;
        [SerializeField] protected SpawnListComponent _particles;

        protected Rigidbody2D _rigidbody;
        protected Vector2 _direction;
        protected Animator _animator;
        protected bool _isGrounded;
        private bool _isJumping;

        private static readonly int IsGroundKey = Animator.StringToHash("is-grounded");
        private static readonly int IsRunning = Animator.StringToHash("is-running");
        private static readonly int VerticalVelocity = Animator.StringToHash("vertical-velocity");
        private static readonly int IsHit = Animator.StringToHash("hit");
        private static readonly int AttackKey = Animator.StringToHash("attack");

        protected virtual void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
        }

        protected virtual void Update()
        {
            _isGrounded = _groundCheck.IsTouchingLayer;
        }

        private void FixedUpdate()
        {
            var xVelocity = _direction.x * _speed;
            var yVelocity = CalculateYVelocity();
            _rigidbody.velocity = new Vector2(xVelocity, yVelocity);

            _animator.SetBool(IsGroundKey, _isGrounded);
            _animator.SetBool(IsRunning, _direction.x != 0);
            _animator.SetFloat(VerticalVelocity, _rigidbody.velocity.y);

            UpdateSpriteDirection();
        }

        protected virtual float CalculateYVelocity()
        {
            var yVelocity = _rigidbody.velocity.y;
            var isJumpPressing = _direction.y > 0;

            if (_isGrounded)
            {
                _isJumping = false;
            }

            if (isJumpPressing)
            {
                _isJumping = true;

                var isFalling = _rigidbody.velocity.y <= 0.001f;
                yVelocity = isFalling ? CalculateJumpVelocity(yVelocity) : yVelocity;
            }
            else if (_rigidbody.velocity.y > 0 && _isJumping)
            {
                yVelocity *= 0.9f;
            }

            return yVelocity;
        }

        protected virtual float CalculateJumpVelocity(float yVelocity)
        {
            if (_isGrounded)
            {
                yVelocity = _jumpSpeed;
                _particles.Spawn("Jump");
            }

            return yVelocity;
        }

        private void UpdateSpriteDirection()
        {
            if (_direction.x > 0)
            {
                //TurnRight
                transform.localScale = Vector3.one;
            }
            else if (_direction.x < 0)
            {
                //TurnLeft
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }

        public virtual void TakeDamage()
        {
            _isJumping = false;
            _animator.SetTrigger(IsHit);
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _damageVelocity);
        }

        public virtual void Attack()
        {
            _animator.SetTrigger(AttackKey);
        }

        public void OnDoAttack()
        {

            var objectsInRange = _attackRange.GetObjectsInRange();
            foreach (var obj in objectsInRange)
            {
                var hp = obj.GetComponent<HealthComponent>();
                if (hp != null)
                {
                    hp.ModifyHealth(-_damage);
                }
            }
        }

    } 
}
