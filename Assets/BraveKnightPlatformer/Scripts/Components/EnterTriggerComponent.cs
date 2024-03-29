using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnterTriggerComponent : MonoBehaviour
{
    [SerializeField] private string _tag;
    [SerializeField] ActionEvent _action;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(_tag))
        {
            Debug.Log("Collision!");
            _action?.Invoke(other.gameObject);
        }
    }

    [Serializable]
    public class ActionEvent : UnityEvent<GameObject> { }
}


