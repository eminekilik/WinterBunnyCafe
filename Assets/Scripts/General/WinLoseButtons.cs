using UnityEngine;
using UnityEngine.SceneManagement;

public class WinLoseButtons : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip buttonSound;

    public void ReplayLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main");

        if (audioSource != null && buttonSound != null)
            audioSource.PlayOneShot(buttonSound);
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;

        LevelData nextLevel = LevelLoader.GetNextLevel();

        if (nextLevel == null)
        {
            Debug.Log("Son level tamamlandý!");
            SceneManager.LoadScene("Menu");
            return;
        }

        LevelLoader.SelectedLevel = nextLevel;
        SceneManager.LoadScene("Main");

        if (audioSource != null && buttonSound != null)
            audioSource.PlayOneShot(buttonSound);
    }
}
