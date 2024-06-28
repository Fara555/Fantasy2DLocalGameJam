using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneFadeInManager : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1.0f; // Длительность растемнения

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
        float elapsedTime = 0.0f; // Время, прошедшее с начала растемнения
        Color color = fadeImage.color; // Начальный цвет изображения

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
