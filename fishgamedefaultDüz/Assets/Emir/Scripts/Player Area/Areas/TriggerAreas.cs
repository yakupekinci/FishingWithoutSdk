using System.Collections;
using UnityEngine;

public class TriggerAreas : MonoBehaviour
{
    protected Coroutine coroutine;

    protected virtual void OnTriggerEnter(Collider other)
    {
        coroutine = StartCoroutine(Counter(other));
    }
    protected virtual void OnTriggerExit(Collider other)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        OnPlayerExit(other);
    }

    protected IEnumerator Counter(Collider other)
    {
        yield return new WaitForSeconds(0.2f);
        OnPlayerEnter(other);
    }

    protected virtual void OnPlayerEnter(Collider other)
    {

    }

    protected virtual void OnPlayerExit(Collider other)
    {
        
    }

    
}
