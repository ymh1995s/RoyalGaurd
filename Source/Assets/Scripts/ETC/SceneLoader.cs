using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    static public void SceneLoad_MainGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    static public void SceneLoad_OverScene()
    {
        SceneManager.LoadScene("GameOverScene");
    }

    static public void SceneLoad_ClearScene()
    {
        SceneManager.LoadScene("GameClearScene");
    }

    static public void SceneLoad_StartScene()
    {
        SceneManager.LoadScene("StartScene");
    }

    static public void GameExit()
    {
        Application.Quit();
    }
}