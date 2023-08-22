using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private UnityEvent _onDamage;
    [SerializeField] private UnityEvent _onDie;
    [SerializeField] private UnityEvent _onHeal;

    public void ApplyDamage(int damageValue)
    {
        if (!gameObject.CompareTag("Enemy"))  return; 

        _health -= damageValue;
        _onDamage?.Invoke();

        // Die
        if (_health <= 0)
        {
            _onDie?.Invoke();
        }

    }

    public void Heal(int healValue)
    {
        _health += healValue;
        _onHeal?.Invoke();
    }
}
