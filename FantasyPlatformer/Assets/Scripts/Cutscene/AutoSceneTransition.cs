using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class AutoSceneTransition : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1.0f; // Длительность затемнения
    public float waitTime = 5.0f; // Время ожидания перед переходом
    public string nextSceneName; // Название следующей сцены

    private void Start()
    {
        StartCoroutine(TransitionAfterTime());
    }

    private IEnumerator TransitionAfterTime()
    {
        // Ожидание завершения катсцены
        yield return new WaitForSeconds(waitTime);
        // Затемнение и переход к следующей сцене
        StartCoroutine(FadeOutAndLoadScene(nextSceneName));
    }

    private IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        float elapsedTime = 0.0f;
        Color color = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }
}
