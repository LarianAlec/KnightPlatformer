using KnightPlatformer.Creatures;
using UnityEngine;
using UnityEngine.InputSystem;


namespace KnightPlatformer
{


    public class HeroInputReader : MonoBehaviour
    {
        [SerializeField] private Hero _hero;

        public void OnMovement(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<Vector2>();
            _hero.SetDirection(direction);
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            // Если мы клавишу отпустили "context.canceled"
            if (context.canceled)
            {
                _hero.Interact();
            }
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _hero.Attack();
            }
        }
    }
}