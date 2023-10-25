using UnityEditor;
using UnityEngine;
using KnightPlatformer.Utils;
using Model;
using KnightPlatformer.Creatures;

namespace KnightPlatformer.Creatures
{
    public class Hero : Creature
    {
        [Space]
        [Header("Interaction check layer")]
        [SerializeField] private LayerMask _interactionLayer;
        [SerializeField] private LayerCheck _wallCheck;

        [Space]
        [Header("Particles")]
        [SerializeField] private float _slamDownVelocity;

        [SerializeField] private float _interactionRadius = 1.0f;
        [SerializeField] private Collider2D[] _interactionResult = new Collider2D[1];

        private bool _allowDoubleJump;
        private bool _isOnWall;

        private GameSession _session;
        private float _defaultGravityScale;

        protected override void Awake()
        {
            base.Awake();
            _defaultGravityScale = _rigidbody.gravityScale;
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

        

        protected override void Update()
        {
            base.Update();

            if (_wallCheck.IsTouchingLayer && _direction.x == transform.localScale.x)
            {
                _isOnWall = true;
                _rigidbody.gravityScale = 0;
            }
            else
            {
                _isOnWall = false;
                _rigidbody.gravityScale = _defaultGravityScale;
            }
        }

       

        protected override float CalculateYVelocity()
        {
            var isJumpPressing = _direction.y > 0;

            if (_isGrounded || _isOnWall)
            {
                _allowDoubleJump = true;
            }

            if (!isJumpPressing && _isOnWall)
            {
                return 0f;
            }
            return base.CalculateYVelocity();
        }

        protected override float CalculateJumpVelocity(float yVelocity)
        {

            if (!_isGrounded && _allowDoubleJump)
            {
                _particles.Spawn("Jump");
                _allowDoubleJump = false;
                return _jumpSpeed;
            }
            return base.CalculateJumpVelocity(yVelocity);
        }


        public void AddCoins(int coins)
        {
            _session.Data.Coins += coins;
            Debug.Log($"{coins} added. Total coins: {_session.Data.Coins}");
        }

        public override void TakeDamage()
        {
            base.TakeDamage();
        }

        public void Interact()
        {
            // Нужно получить персечение со всеми пересекающимися объектами
            var size = Physics2D.OverlapCircleNonAlloc(
                gameObject.transform.position,
                _interactionRadius,
                _interactionResult,
                _interactionLayer);

            for (int i = 0; i < size; i++)
            {
                var interactable = _interactionResult[i].GetComponent<InteractableComponent>();
                if (interactable != null)
                {
                    interactable.Interact();
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.IsInLayer(_groundLayer))
            {
                // collision velocity
                var contact = other.contacts[0];
                if (contact.relativeVelocity.y >= _slamDownVelocity)
                {
                    _particles.Spawn("SlamDown");
                }
            }
        }

        public override void Attack()
        {
            base.Attack();
        }
    }
}