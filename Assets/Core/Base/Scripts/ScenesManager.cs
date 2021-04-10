using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float fadeOutDelay;
    [SerializeField] float menuFadeTime;

    [Header("References")]
    [SerializeField] CanvasGroup menuCG;

    public static ScenesManager Instance;

    Coroutine menuCO;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else Destroy(gameObject);
    }

    // Переключаем сцену
    public void LoadScene(int sceneIndex)
    {
        if (menuCO != null)
            StopCoroutine(menuCO);

        menuCO = StartCoroutine(StartSwitching(sceneIndex));
    }

    // Анимация Fade-эффекта
    IEnumerator StartSwitching(int sceneIndex)
    {
        float elapsedTime = 0;
        menuCG.interactable = true;
        menuCG.blocksRaycasts = true;

        // Fade in
        while (elapsedTime < menuFadeTime)
        {
            elapsedTime += Time.deltaTime;
            menuCG.alpha = Mathf.Lerp(0, 1, elapsedTime / menuFadeTime);

            yield return null; ;
        }

        elapsedTime = 0;
        SceneManager.LoadScene(sceneIndex);

        // Fade delay
        yield return new WaitForSeconds(fadeOutDelay);

        menuCG.interactable = false;
        menuCG.blocksRaycasts = false;

        // Fade out
        while (elapsedTime < menuFadeTime)
        {
            elapsedTime += Time.deltaTime;
            menuCG.alpha = Mathf.Lerp(1, 0, elapsedTime / menuFadeTime);

            yield return null; ;
        }

        menuCG.alpha = 0;
    }
}
