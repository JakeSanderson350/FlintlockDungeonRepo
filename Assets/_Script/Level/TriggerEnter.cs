using System;
using UnityEngine;

public class TriggerEnter : MonoBehaviour
{
    [SerializeField] string triggerTag = "Player";
    public Action onTriggerEntered;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == triggerTag)
        {
            enabled = false;
            onTriggerEntered?.Invoke();
        }
    }
}
