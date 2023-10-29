using System.Collections;
using UnityEngine;

public class TestCoroutineScript : MonoBehaviour
{
    [SerializeField] private Coroutine _routine;
    private void Start()
    {
        _routine = StartCoroutine(TestCoroutine());
    }

    [ContextMenu("Kill routine")]
    private void KillRoutine()
    {
        StopCoroutine(_routine);
    }

    [ContextMenu("Start routine")]
    private void StartRoutine()
    {
        StartCoroutine(TestCoroutine());
    }

    private IEnumerator TestCoroutine()
    {
        Debug.Log("start coroutine");
        while (enabled)
        {
            Debug.Log("waiting for another coroutine");
            yield return AnotherCoroutine();
            Debug.Log("is done");
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator AnotherCoroutine()
    {
        yield return new WaitForSeconds(2f);
    }
}
