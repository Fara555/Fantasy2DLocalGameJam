using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneFadeInManager : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1.0f; // ������������ �����������

    private void Start()
    {
        
        if (fadeImage != null)
        {
            
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1);
            
            StartCoroutine(FadeIn());
        }
        else
        {
            Debug.LogError("Fade Image is not set in the inspector.");
        }
    }

    
    private IEnumerator FadeIn()
    {
        float elapsedTime = 0.0f; // �����, ��������� � ������ �����������
        Color color = fadeImage.color; // ��������� ���� �����������

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = 1.0f - Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        
        color.a = 0.0f;
        fadeImage.color = color;
    }
}
