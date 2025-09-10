using System;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEnter : MonoBehaviour
{
    [SerializeField] string triggerTag = "Player";
    public Action onTriggerEntered;
    
    [SerializeField] UnityEvent onTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == triggerTag)
        {
            enabled = false;
            onTriggerEntered?.Invoke();
            onTrigger?.Invoke();
        }
    }
}
