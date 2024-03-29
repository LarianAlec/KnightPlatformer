using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchComponent : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private bool _state;
    [SerializeField] private string _animationKey = "is-opened";
    public void Switch()
    {
        _state = !_state;
        _animator.SetBool(_animationKey, _state);
    }

    [ContextMenu("Switch")]
    public void SwitchIt()
    {
        Switch();
    }

}
