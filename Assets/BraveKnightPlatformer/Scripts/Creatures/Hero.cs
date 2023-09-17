using UnityEditor;
using UnityEngine;
using KnightPlatformer.Utils;
using Model;
using KnightPlatfomer.Creatures;

namespace KnightPlatformer.Creatures
{
    public class Hero : Creature
    {
        [Space]
        [Header("Interaction check layer")]
        [SerializeField] private float _interactionRadius = 1.0f;
        [SerializeField] private Collider2D[] _interactableResult = new Collider2D[1];
        [SerializeField] private LayerMask _interactionLayer;

        [Space]
        [Header("Particles")]
        [SerializeField] private SpawnComponent _footStepParticle;
        [SerializeField] private SpawnComponent _jumpParticle;
        [SerializeField] private SpawnComponent _slamDownParticle;
        [SerializeField] private float _slamDownVelocity;

        private bool _allowDoubleJump;
        private bool _isGrounded;
        private bool _isJumping;

        private Vector2 _direction;
        private Rigidbody2D _rigidbody;

        private Animator _animator;
        private static readonly int IsGroundKey = Animator.StringToHash("is-grounded");
        private static readonly int IsRunning = Animator.StringToHash("is-running");
        private static readonly int VerticalVelocity = Animator.StringToHash("vertical-velocity");
        private static readonly int IsHit = Animator.StringToHash("hit");
        private static readonly int AttackKey = Animator.StringToHash("attack");


        private GameSession _session;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();

            var health = GetComponent<HealthComponent>();
            health.SetHealth(_session.Data.Hp);
        }

        public void OnHealthChanged(int currentHealth)
        {
            _session.Data.Hp = currentHealth;
        }

        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
        }

        private void Update()
        {
            _isGrounded = IsGrounded();
        }

       

        private float CalculateYVelocity()
        {
            var yVelocity = _rigidbody.velocity.y;
            var isJumpPressing = _direction.y > 0;

            if (_isGrounded)
            {
                _allowDoubleJump = true;
                _isJumping = false;
            }

            if (isJumpPressing)
            {
                _isJumping = true;
                yVelocity = CalculateJumpVelocity(yVelocity);
            }
            else if (_rigidbody.velocity.y > 0 && _isJumping)
            {
                yVelocity *= 0.9f;
            }

            return yVelocity;
        }

        private float CalculateJumpVelocity(float yVelocity)
        {
            var isFalling = _rigidbody.velocity.y <= 0.001f;
            if (!isFalling) return yVelocity;

            if (_isGrounded)
            {
                yVelocity += _jumpSpeed;
                _jumpParticle.Spawn();
            }
            else if (_allowDoubleJump)
            {
                yVelocity = _jumpSpeed;
                _jumpParticle.Spawn();
                _allowDoubleJump = false;
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

        private bool IsGrounded()
        {
            var hit = Physics2D.CircleCast(transform.position + _groundCheckPositionDelta, _groundCheckRadius, Vector2.down, 0, _groundLayer);
            return hit.collider != null;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Handles.color = IsGrounded() ? HandlesUtils.TransparentGreen : HandlesUtils.TransparentRed;
            Handles.DrawSolidDisc(transform.position + _groundCheckPositionDelta, Vector3.forward, _groundCheckRadius);
        }
#endif

        public void AddCoins(int coins)
        {
            _session.Data.Coins += coins;
            Debug.Log($"{coins} added. Total coins: {_session.Data.Coins}");
        }

        public void TakeDamage()
        {
            _isJumping = false;
            _animator.SetTrigger(IsHit);
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _damageVelocity);
        }

        public void Interact()
        {
            // Нужно получить персечение со всеми пересекающимися объектами
            var size = Physics2D.OverlapCircleNonAlloc(gameObject.transform.position, _interactionRadius, _interactableResult, _interactionLayer);
            for (int i = 0; i < size; i++)
            {
                var interactable = _interactableResult[i].GetComponent<InteractableComponent>();
                if (interactable != null)
                {
                    interactable.Interact();
                }
            }
        }

        public void StartAttackAnimation()
        {
            Debug.Log("Attack Animation!");
            _animator.SetTrigger(AttackKey);
        }

        public void Attack()
        {
            Debug.Log("Attack!");
            _rigidbody.velocity = Vector2.zero;
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

        public void SpawnFootStepDust()
        {
            _footStepParticle.Spawn();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.IsInLayer(_groundLayer))
            {
                // collision velocity
                var contact = other.contacts[0];
                if (contact.relativeVelocity.y >= _slamDownVelocity)
                {
                    _slamDownParticle.Spawn();
                }
            }
        }

    }
}