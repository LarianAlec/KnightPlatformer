using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeroInputReader : MonoBehaviour
{
    private Hero _hero;
    private Vector2 _direction;

    private void Awake()
    {
        _hero = GetComponent<Hero>();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        _direction = context.ReadValue<Vector2>();
        _hero.SetDirection(_direction);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        // Если мы клавишу отпустили "context.canceled"
        if (context.canceled)
        {
            _hero.Interact();
        }
    }
}
