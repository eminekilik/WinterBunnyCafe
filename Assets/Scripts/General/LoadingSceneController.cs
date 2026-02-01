using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingSceneController : MonoBehaviour
{
    public Slider loadingSlider;
    float fakeProgress = 0f;

    void Start()
    {
        StartCoroutine(LoadMainSceneAsync());
    }

    IEnumerator LoadMainSceneAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("Main");
        operation.allowSceneActivation = false;

        while (operation.progress < 0.9f)
        {
            fakeProgress += Time.deltaTime * 0.3f;
            fakeProgress = Mathf.Min(fakeProgress, operation.progress);

            loadingSlider.value = fakeProgress;
            yield return null;
        }

        // son %10'u yumuþak doldur
        while (loadingSlider.value < 1f)
        {
            loadingSlider.value += Time.deltaTime * 0.5f;
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);
        operation.allowSceneActivation = true;
    }
}
