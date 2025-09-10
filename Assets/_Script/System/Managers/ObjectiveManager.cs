using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectiveManager : MonoBehaviour
{
    public static List<Objective> objectives = new();

    private void OnEnable()
    {
        EventManager.onSceneLoaded += ScrubObjectiveList;
    }
    private void OnDisable()
    {
        EventManager.onSceneLoaded -= ScrubObjectiveList;
    }

    void ScrubObjectiveList(LevelData data)
    {
        //scrub out null objectives from unloaded scenes, not needed if objectives are implimented correctly 
        objectives.RemoveAll(x => x == null);
        foreach (Objective obj in objectives) { obj.Group.triggered = false; }
    }

    public static void CheckObjectiveStatus(ObjectiveGroup group)
    {
        //if the group has it's conditions met then trigger it's callbacks
        if (GetObjectiveStatus(group)) { group.TriggerObjective(); }
    }

    public static bool GetObjectiveStatus(ObjectiveGroup group)
    {
        List<Objective> checkList = objectives.FindAll(x => x.Group == group);

        switch (group.condition)
        {
            //True when num of activated objectives is zero
            case ObjectiveGroup.TriggerCondition.NONE:
                if (checkList.FindAll(x => x.IsActivated).Count <= 0) return true;
                break;

            //True when no inactive objectives can be found
            case ObjectiveGroup.TriggerCondition.ALL:
                if (checkList.Find(x => !x.IsActivated) == null) return true;
                break;

            //True when the number of activated objectives is greater than zero
            case ObjectiveGroup.TriggerCondition.ANY:
                if (checkList.FindAll(x => x.IsActivated).Count > 0) return true;
                break;

            //True when the number of activated objectives is at or more than the group's threshold 
            case ObjectiveGroup.TriggerCondition.SOME:
                if (checkList.FindAll(x => x.IsActivated).Count >= group.thresholdNum) return true;
                break;
        }

        return false;
    }
}
