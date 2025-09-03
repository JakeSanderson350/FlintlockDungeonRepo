using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "new level", menuName = "Level Data")]
public class LevelData : ScriptableObject
{
    [Header("Header")]
    public string sceneName;
    public new string name;
    [TextArea] public string description;

    [Header("Victory Conditions")]
    public LevelData nextLevel;

    [Header("Audio")]
    public bool playOnSceneLoad;
    public AudioClip soundtrack;

    public void LoadNextLevel() => SceneManager.LoadScene(nextLevel.sceneName);
}
