using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyComponent : MonoBehaviour
{
    [SerializeField] private GameObject _objectToDestroy;

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void DestroyObject()
    {
        Destroy(_objectToDestroy);
    }
}
