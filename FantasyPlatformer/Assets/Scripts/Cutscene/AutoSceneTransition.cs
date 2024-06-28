using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class AutoSceneTransition : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1.0f; // ������������ ����������
    public float waitTime = 5.0f; // ����� �������� ����� ���������
    public string nextSceneName; // �������� ��������� �����

    private void Start()
    {
        StartCoroutine(TransitionAfterTime());
    }

    private IEnumerator TransitionAfterTime()
    {
        // �������� ���������� ��������
        yield return new WaitForSeconds(waitTime);
        // ���������� � ������� � ��������� �����
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
