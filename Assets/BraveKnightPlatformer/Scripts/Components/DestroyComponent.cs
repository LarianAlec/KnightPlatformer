using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyComponent : MonoBehaviour
{
    [SerializeField] private GameObject _object;

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void DestroyObject()
    {
        Destroy(_object);
    }
}
