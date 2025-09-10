using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "new level", menuName = "Level/Level Data")]
public class LevelData : ScriptableObject
{
    [Header("Header")]
    public string sceneName;
    public new string name;
    [TextArea] public string description;

    [Header("Level Links")]
    public LevelData nextLevel;
    public LevelData lastLevel;
    public LevelData reloadLevel;

    [Header("Misc")]
    public bool enableMouseOnLoad;
    public bool playOnSceneLoad;
    public AudioClip soundtrack;

    public void LoadNextLevel() => SceneManager.LoadScene(nextLevel.sceneName);
    public void LoadLastLevel() => SceneManager.LoadScene(lastLevel.sceneName);
    public void ReloadLevel() => SceneManager.LoadScene(reloadLevel.sceneName);
}
