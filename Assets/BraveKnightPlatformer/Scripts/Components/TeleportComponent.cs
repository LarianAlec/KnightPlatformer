using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportComponent : MonoBehaviour
{
    [SerializeField] private Transform _transformDestination;

    public void Teleport(GameObject target)
    {

        StartCoroutine(Teleporting(target));
    }

    IEnumerator Teleporting(GameObject target)
    {
        yield return new WaitForSeconds(0.5f);
        target.transform.position = _transformDestination.position;
    }
}
