using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Sahne ismine göre geçiþ
    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Build Settings sýrasýna göre geçiþ
    public void LoadSceneByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    // Oyundan çýkýþ
    public void QuitGame()
    {
        Application.Quit();
    }
}
