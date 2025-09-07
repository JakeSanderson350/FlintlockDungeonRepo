using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FalseFloor : MonoBehaviour
{
    [SerializeField] private float destroyDelay = 1f;
    private bool hasBeenTouched = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !hasBeenTouched)
        {
            hasBeenTouched = true;
            StartCoroutine(DestroyAfterDelay());
        }
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
}
