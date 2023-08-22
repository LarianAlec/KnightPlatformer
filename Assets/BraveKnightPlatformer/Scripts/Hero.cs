using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using KnightPlatformer.Utils;

public class Hero : MonoBehaviour
{
    [Space] [Header("Properties")]
    [SerializeField] private float _jumpSpeed = 10f;
    [SerializeField] private float _jumpOnDamage = 5f;
    [SerializeField] private float _speed = 5.0f;
    [SerializeField] private int _damage = 1;

    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Vector3 _groundCheckPositionDelta = new Vector3(0.03f, 0.16f, 0);
    [SerializeField] private float _groundCheckRadius = 0.25f;
    [SerializeField] private float _interactionRadius = 1.0f;
    [SerializeField] private Collider2D[] _interactableResult = new Collider2D[1];
    [SerializeField] private LayerMask _interactionLayer;

    [SerializeField] private CheckCircleOverlap _attackRange;

    [Space] [Header("Particles")]
    [SerializeField] private SpawnComponent _footStepParticle;
    [SerializeField] private SpawnComponent _jumpParticle;
    [SerializeField] private SpawnComponent _slamDownParticle;
    [SerializeField] private float _slamDownVelocity;

    private bool _allowDoubleJump;
    private bool _isGrounded;

    private Vector2 _moveDirection;
    private Rigidbody2D _rigidbody;

    private Animator _animator;
    private static readonly int IsGroundKey = Animator.StringToHash("is-grounded");
    private static readonly int IsRunning = Animator.StringToHash("is-running");
    private static readonly int VerticalVelocity = Animator.StringToHash("vertical-velocity");
    private static readonly int IsHit = Animator.StringToHash("hit");
    private static readonly int AttackKey = Animator.StringToHash("attack");

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    public void SetDirection(Vector2 direction)
    {
        _moveDirection = direction;
    }

    private void Update()
    {
        _isGrounded = IsGrounded();
    }

    private void FixedUpdate()
    {
        var xVelocity = _moveDirection.x * _speed;
        var yVelocity = CalculateYVelocity();
        _rigidbody.velocity = new Vector2(xVelocity, yVelocity);

        _animator.SetBool(IsGroundKey, IsGrounded());
        _animator.SetBool(IsRunning, _moveDirection.x != 0);
        _animator.SetFloat(VerticalVelocity, _rigidbody.velocity.y);

        UpdateSpriteDirection();
    }

    private float CalculateYVelocity()
    {
        var yVelocity = _rigidbody.velocity.y;
        var isJumpPressing = _moveDirection.y > 0;

        if (_isGrounded) _allowDoubleJump = true;

        if (isJumpPressing)
        {
            yVelocity = CalculateJumpVelocity(yVelocity);
        }
        else if (_rigidbody.velocity.y > 0)
        {
            yVelocity *= 0.95f;
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
        if (_moveDirection.x > 0)
        {
            //TurnRight
            transform.localScale = Vector3.one;
        }
        else if (_moveDirection.x < 0)
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

    private void OnDrawGizmos()
    {
        Gizmos.color = IsGrounded() ? Color.green : Color.red;
        Gizmos.DrawSphere(transform.position + _groundCheckPositionDelta, _groundCheckRadius);
    }

    public void TakeDamage()
    {
        _animator.SetTrigger(IsHit);
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpOnDamage);
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
        foreach(var obj in objectsInRange)
        {
            var hp = obj.GetComponent<HealthComponent>();
            if (hp != null)
            {
                hp.ApplyDamage(_damage);
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
