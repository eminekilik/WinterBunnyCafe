using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class LoadingSceneController : MonoBehaviour
{
    public Slider loadingSlider;
    float fakeProgress = 0f;

    public TMP_Text loadingText;
    bool isLoading = true;

    void Start()
    {
        StartCoroutine(LoadingTextAnimation());
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
        isLoading = false;
        operation.allowSceneActivation = true;
    }

    IEnumerator LoadingTextAnimation()
    {
        string baseText = "Loading";

        int dotCount = 0;

        while (isLoading)
        {
            dotCount = (dotCount + 1) % 4; // 0-3 arasý
            loadingText.text = baseText + new string('.', dotCount);

            yield return new WaitForSeconds(0.4f);
        }
    }

}
