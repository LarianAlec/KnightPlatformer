using System.Collections;
using UnityEngine;

namespace KnightPlatformer.Creatures
{
    public class MobAI : MonoBehaviour
    {
        [SerializeField] LayerCheck _vision;
        [SerializeField] LayerCheck _canAttack;
        [SerializeField] float _alarmDelay = 4.0f;
        [SerializeField] float _attackCooldown = 0.3f;

        private Coroutine _current;
        private GameObject _target;
        private Creature _creature;

        private Components.SpawnListComponent _particles;

        private bool _isDead = false;
        private static readonly int DeadKey = Animator.StringToHash("is-dead");
        private Animator _animator;
        private Patrol _patrol;

        private void Awake()
        {
            _particles = GetComponent<Components.SpawnListComponent>();
            _creature = GetComponent<Creature>();
            _animator = GetComponent<Animator>();
            _patrol = GetComponent<Patrol>();
        }

        private void Start()
        {
            StartState(_patrol.DoPatrol());
        }

        private void StartState(IEnumerator coroutine)
        {
            _creature.SetDirection(Vector2.zero);

            if (_current != null)
                StopCoroutine(_current);

            _current = StartCoroutine(coroutine);
        }

        public void OnHeroInVision(GameObject go)
        {
            if (_isDead) return;

            _target = go;
            StartState(AgroToHero());
        }

        private IEnumerator AgroToHero()
        {
            _particles.Spawn("Exclamation");
            yield return new WaitForSeconds(_alarmDelay);

            StartState(GoToHero());
        }

        private IEnumerator GoToHero()
        {
            while (_vision.IsTouchingLayer)
            {
                if (_canAttack.IsTouchingLayer)
                {
                    StartState(Attack());
                } 
                else
                {
                    SetDirectionToTarget();
                }
                yield return null;
            }

            StartState(_patrol.DoPatrol());
        }

        private void SetDirectionToTarget()
        {
            var direction = _target.transform.position - transform.position;
            direction.y = 0;
            _creature.SetDirection(direction.normalized);
        }

        private IEnumerator Attack()
        {
            while (_canAttack.IsTouchingLayer)
            {
                _creature.Attack();
                yield return new WaitForSeconds(_attackCooldown);
            }

            StartState(GoToHero());
        }

        public void Die()
        {
            _isDead = true;
            _animator.SetBool(DeadKey, _isDead);

            if (_current != null)
                StopCoroutine(_current);
        }
    }
}