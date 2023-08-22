using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DelayComponent : MonoBehaviour
{
    [SerializeField] private float _delay = 1.0f;
    [SerializeField] private UnityEvent _eventToInvokeWithDelay;

    private void InvokeEventWithDelay()
    {
        if (_eventToInvokeWithDelay != null)
        {
            Invoke("InvokeEvent", _delay);
        }
    }

    private void Start()
    {
        InvokeEventWithDelay();
    }

    //======== Methods to Invoke =======

    private void InvokeEvent()
    {
        _eventToInvokeWithDelay.Invoke();
    }

    public void DeleteSelf()
    {
        Destroy(gameObject);
    }
}
