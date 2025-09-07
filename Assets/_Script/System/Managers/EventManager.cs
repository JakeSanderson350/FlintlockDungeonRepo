using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    // SYSTEM //


    // UI //


    // PLAYER //
    public static void PlayerDied() => onPlayerDied?.Invoke(); //Example event
    public static event Action onPlayerDied; //Example signal
    
    // WORLD //
    public static void EncounterObjective(IEncounterObjective objective) => onEncounterObjective?.Invoke(objective);
    public static event Action<IEncounterObjective> onEncounterObjective;
}
