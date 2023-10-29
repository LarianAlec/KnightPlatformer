using UnityEngine;
using UnityEngine.Events;

namespace KnightPlatformer.Components
{
    public class InteractableComponent : MonoBehaviour
    {
        [SerializeField] private UnityEvent _action;

        public void Interact()
        {
            _action?.Invoke();
        }

    }
}
