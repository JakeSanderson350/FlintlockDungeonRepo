using UnityEngine;


[CreateAssetMenu(fileName = "new player preferences", menuName = "Player/Player Preferences")]
public class PlayerPrefs : ScriptableObject
{
    [Header("Camera")]
    [SerializeField, Range(0, 5)] public float sentitivityX = 1f;
    [SerializeField, Range(0, 5)] public float sentitivityY = 0.8f;
    [SerializeField] public bool invertX;
    [SerializeField] public bool invertY;
}
