using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Transitions : MonoBehaviour
{
    public static Transitions Instance;

    void Awake()
    {
        Instance = this;
    }

    // Анимация Fade In/Out
    public IEnumerator Fade(CanvasGroup canvasGroup, float duration, bool show)
    {
        float startValue = show ? 0 : 1;
        float finalValue = show ? 1 : 0;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startValue, finalValue, elapsedTime / duration);

            yield return null;
        }

        canvasGroup.alpha = finalValue;
    }

    // Анимация изменения скейла объекта
    public IEnumerator ChangeScale(RectTransform rectTransform, Vector2 startScale, float scaleMultiplier, float duration)
    {
        Vector2 finalScale = startScale * scaleMultiplier;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            rectTransform.localScale = Vector2.Lerp(startScale, finalScale, elapsedTime / duration);

            yield return null;
        }

        rectTransform.localScale = finalScale;
    }

    // Анимация изменения скейла объекта
    public IEnumerator ChangeScale(RectTransform rectTransform, Vector2 startScale, Vector2 finalScale, float duration)
    {
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            rectTransform.localScale = Vector2.Lerp(startScale, finalScale, elapsedTime / duration);

            yield return null;
        }

        rectTransform.localScale = finalScale;
    }

    // Анимация смены цвета
    public IEnumerator LerpColor(Image image, Color a, Color b, float duration)
    {
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            image.color = Color.Lerp(a, b, elapsedTime / duration);

            yield return null;
        }

        image.color = b;
    }
}
