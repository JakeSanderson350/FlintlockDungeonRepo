using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour 
{
    public void LoadScene(string name)
    {
        GameManager.inst.LoadScene(name);
    }

    public void ReloadScene()
    {
        GameManager.inst.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        GameManager.inst.QuitGame();
    }

    public void LoadNextLevel()
    {
        GameManager.inst.LoadNextLevel();
    }
}
