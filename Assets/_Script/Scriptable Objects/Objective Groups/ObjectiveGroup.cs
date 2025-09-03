using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New ObjectiveGroup", menuName = "Objective/ObjectiveGroup")]
public class ObjectiveGroup : ScriptableObject
{
    public enum TriggerCondition { ALL, ANY, NONE, SOME }
    //public enum GameState { Win, Lose } //imperfect

    public string groupName = "Objective 1";
    public TriggerCondition condition;
    public event Action onObjectiveTriggered;
    public int thresholdNum;

    public bool triggered = false;
    
    public void TriggerObjective()
    {
        if(triggered)
            return; //each objective can only be triggered once 

        triggered = true;
        Debug.Log("Objective Group \"" + name + "\" Triggered");
        onObjectiveTriggered?.Invoke();
    }

    
}
