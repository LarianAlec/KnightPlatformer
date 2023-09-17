using UnityEngine;

namespace KnightPlatfomer.Creatures
{
    public class Creature : MonoBehaviour
    {
        [Space]
        [Header("Properties")]
        [SerializeField] private float _speed = 5.0f;
        [SerializeField] private float _jumpSpeed = 10f;
        [SerializeField] private float _damageVelocity = 5f;
        [SerializeField] private int _damage = 1;

        [Space]
        [Header("Ground check layer")]
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private Vector3 _groundCheckPositionDelta = new Vector3(0.03f, 0.16f, 0);
        [SerializeField] private float _groundCheckRadius = 0.25f;

        [SerializeField] private CheckCircleOverlap _attackRange;
    }
}