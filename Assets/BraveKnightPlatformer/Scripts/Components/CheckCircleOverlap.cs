using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class CheckCircleOverlap : MonoBehaviour
{
    [SerializeField] private float _radius = 1f;
    [SerializeField] private LayerMask _mask;
    [SerializeField] private string[] _tags;
    [SerializeField] private OnOverlapEvent _onOverlap;
    private readonly Collider2D[] _interactionResult = new Collider2D[10];

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = KnightPlatformer.Utils.HandlesUtils.TransparentRed;
        Handles.DrawSolidDisc(gameObject.transform.position, Vector3.forward, _radius);
    }
#endif

    public void Check()
    {
        var size = Physics2D.OverlapCircleNonAlloc(
            gameObject.transform.position,
            _radius,
            _interactionResult,
            _mask);

        var overlaps = new List<GameObject>();

        for (int i = 0; i < size; i++)
        {
            var overlapResult = _interactionResult[i];
            bool IsInTags = _tags.Any(tag => overlapResult.CompareTag(tag));
            if (IsInTags)
            {
                _onOverlap?.Invoke(overlapResult.gameObject);
            }
        }

    }

    [Serializable]
    public class OnOverlapEvent : UnityEvent<GameObject>
    {
    }
}
