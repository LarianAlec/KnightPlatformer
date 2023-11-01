using System;
using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private UnityEvent _onDamage;
    [SerializeField] private UnityEvent _onDie;
    [SerializeField] private UnityEvent _onHeal;
    [SerializeField] private HealthChangeEvent _onChange;

    private bool _isDead = false;

    public void ModifyHealth(int healthDelta)
    {
        _health += healthDelta;
        _onChange?.Invoke(_health);

        if (healthDelta < 0 && !_isDead)
        {
            _onDamage?.Invoke();
        }

        if (healthDelta > 0)
        {
            _onHeal?.Invoke();
        }

        if (_health <=0 && !_isDead)
        {
            _onDie?.Invoke();
            _isDead = true;
        }
    }

    public void SetHealth(int health)
    {
        _health = health;
    }

    [Serializable]
    public class HealthChangeEvent : UnityEvent<int> 
    {
    }
}
