using UnityEngine;

public class Objective : MonoBehaviour
{
    public ObjectiveGroup Group { get { return Group; } } 
    [SerializeField] ObjectiveGroup group;
    public bool IsActivated { get { return isActivated; } set { SetObjective(value); } }
    bool isActivated;
    
    void OnEnable()
    {
        ObjectiveManager.objectives.Add(this);
    }
    void OnDisable()
    {
        ObjectiveManager.objectives.Remove(this);
    }

    void SetObjective(bool value)
    {
        isActivated = value;
        ObjectiveManager.CheckObjectiveStatus(group);
    }
}

