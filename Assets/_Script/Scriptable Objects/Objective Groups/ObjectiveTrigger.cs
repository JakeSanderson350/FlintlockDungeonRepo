using UnityEngine;

public class ObjectiveTrigger : MonoBehaviour
{
    public Objective obj;
    public string triggerTag;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == triggerTag || triggerTag == "")
            Trigger();
    }

    void Trigger()
    {
        obj.IsActivated = true;
    }

}
