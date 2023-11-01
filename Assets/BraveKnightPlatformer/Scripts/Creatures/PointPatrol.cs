using System.Collections;
using UnityEngine;

namespace KnightPlatformer.Creatures
{
    public class PointPatrol : Patrol
    {
        [SerializeField] private Transform[] _points;
        [SerializeField] private float _threshold = 1f;



        private Creature _creature;
        private int _destinationPointIndex;

        private void Awake()
        {
            _creature = GetComponent<Creature>();
        }

        public override IEnumerator DoPatrol()
        {
            // проверяем каждый кадр на приближение к точке
            while (enabled)
            {
                if (IsOnPoint())
                {
                    // переход на след точку
                    _destinationPointIndex = (int) Mathf.Repeat(_destinationPointIndex + 1, _points.Length);
                }

                var direction = _points[_destinationPointIndex].position - transform.position;
                direction.y = 0f;
                _creature.SetDirection(direction.normalized);

                yield return null;
            }
        }

        private bool IsOnPoint()
        {
            return (_points[_destinationPointIndex].position - transform.position).magnitude < _threshold;
        }
    }
}
