using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager inst;
    private List<LevelData> levelData;
    [HideInInspector] public LevelData currentLevel;

    private void Awake()
    {
        if(inst != null)
        {
            Destroy(gameObject);
            return;
        }

        transform.parent = null;
        DontDestroyOnLoad(gameObject);
        inst = this;

        levelData = Resources.LoadAll<LevelData>("LevelData/").ToList();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneLoaded;
    }

    /// <summary> Master controler for loading scenes. Pls use this instead of scene manager </summary>

    public void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1.0f;
        currentLevel = levelData.Find(x => x.sceneName == SceneManager.GetActiveScene().name);
        if (currentLevel == null) Debug.LogError("No scene data found in \"Resources/LevelData\" for current scene");
        EventManager.SceneLoaded(currentLevel);

        Cursor.visible = currentLevel.enableMouseOnLoad;
        Cursor.lockState = currentLevel.enableMouseOnLoad ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void ReloadScene()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadNextLevel()
    {
        currentLevel.LoadNextLevel();
    }
    public void LoadLastLevel()
    {
        currentLevel.LoadNextLevel();
    }
    public void ReloadLevel()
    {
        currentLevel.ReloadLevel();
    }
}

