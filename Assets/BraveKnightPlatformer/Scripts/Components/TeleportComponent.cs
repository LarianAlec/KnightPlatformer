using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportComponent : MonoBehaviour
{
    [SerializeField] private Transform _transformDestination;
    [SerializeField] private float _delay = 0.3f;

    public void Teleport(GameObject target)
    {

        StartCoroutine(Teleporting(target));
    }

    IEnumerator Teleporting(GameObject target)
    {
        yield return new WaitForSeconds(_delay);
        target.transform.position = _transformDestination.position;
    }
}
