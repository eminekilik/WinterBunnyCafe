using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip buttonSound;

    // Sahne ismine göre geçiþ
    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        if (audioSource != null && buttonSound != null)
            audioSource.PlayOneShot(buttonSound);
    }

    // Build Settings sýrasýna göre geçiþ
    public void LoadSceneByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
        if (audioSource != null && buttonSound != null)
            audioSource.PlayOneShot(buttonSound);
    }

    public void LoadMenu()
    {
        LevelButton[] buttons = FindObjectsOfType<LevelButton>();

        foreach (var btn in buttons)
        {
            btn.UpdateState();
        }

        SceneManager.LoadScene("Menu");
        if (audioSource != null && buttonSound != null)
            audioSource.PlayOneShot(buttonSound);
    }

    // LevelData ile oyun sahnesi yükleme
    public void LoadLevel(LevelData levelData)
    {
        Time.timeScale = 1f;
        LevelLoader.SelectedLevel = levelData;
        SceneManager.LoadScene("Main"); // oyun sahnenin adý
        if (audioSource != null && buttonSound != null)
            audioSource.PlayOneShot(buttonSound);
    }

    // Oyundan çýkýþ
    public void QuitGame()
    {
        Application.Quit();
    }
}
