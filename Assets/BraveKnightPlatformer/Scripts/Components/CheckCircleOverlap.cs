using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CheckCircleOverlap : MonoBehaviour
{
    [SerializeField] private float _radius = 1f;

    private readonly Collider2D[] _attackResult = new Collider2D[5];

    public GameObject[] GetObjectsInRange()
    {
        var size = Physics2D.OverlapCircleNonAlloc(gameObject.transform.position, _radius, _attackResult);
        var overlaps = new List<GameObject>();

        for (int i = 0; i < size; i++)
        {
            overlaps.Add(_attackResult[i].gameObject);
        }

        return overlaps.ToArray();
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = KnightPlatformer.Utils.HandlesUtils.TransparentRed;
        Handles.DrawSolidDisc(gameObject.transform.position, Vector3.forward, _radius);
    }
#endif
}
