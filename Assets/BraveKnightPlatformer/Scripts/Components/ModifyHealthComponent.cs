using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyHealthComponent : MonoBehaviour
{
    [SerializeField] private int _healValue = 3;
    [SerializeField] private int _damageValue = 1;

    public void ApplyDamage(GameObject target)
    {
        var healthComponent = target.GetComponent<HealthComponent>();
        if (healthComponent != null)
        {
            healthComponent.ModifyHealth(-_damageValue);
        }
    }

    public void Heal(GameObject target)
    {
        var healthComponent = target.GetComponent<HealthComponent>();
        if (healthComponent != null)
        {
            healthComponent.ModifyHealth(_healValue);
        }
    }
}
