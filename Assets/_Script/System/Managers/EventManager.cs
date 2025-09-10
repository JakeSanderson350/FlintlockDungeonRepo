using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    // SYSTEM //
    public static void SceneLoaded(LevelData data) => onSceneLoaded?.Invoke(data);
    public static event Action<LevelData> onSceneLoaded;

    // UI //


    // PLAYER //
    public static void PlayerDied() => onPlayerDied?.Invoke(); //Example event
    public static event Action onPlayerDied; //Example signal

    public static void SetCameraTrauma(float trauma) => onSetCameraTrauma?.Invoke(trauma);
    public static event Action<float> onSetCameraTrauma;
    
    // WORLD //
    public static void EncounterObjective(IEncounterObjective objective) => onEncounterObjective?.Invoke(objective);
    public static event Action<IEncounterObjective> onEncounterObjective;
}
