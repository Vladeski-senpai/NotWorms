using DG.Tweening;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float adBtnShakeDelay;
    [SerializeField] float adBtnShakeDuration;
    [SerializeField] Vector2 adBtnShakeVector;
    //[SerializeField] float adBtnShakeStrength;
    [SerializeField] int adBtnShakeVibrato;
    [SerializeField] float adBtnShakeRandomness;
    [SerializeField] float menuFadeTime;
    [SerializeField] float menuFadeInDelay;

    [Header("References")]
    [SerializeField] CanvasGroup menuCG;
    [SerializeField] RectTransform adButtonRT;

    Transitions transitions;
    Coroutine menuCO;
    Tweener adTweener;
    Helper helper;

    void Start()
    {
        transitions = Transitions.Instance;
        helper = Helper.Instance;
    }

    // Включаем выключаем меню
    public void MenuState(bool show)
    {
        menuCG.interactable = show;
        menuCG.blocksRaycasts = show;

        if (show)
        {
            helper.PerformWithDelay(adBtnShakeDelay, () =>
            {
                adTweener = adButtonRT.DOShakeAnchorPos(adBtnShakeDuration, adBtnShakeVector, adBtnShakeVibrato, adBtnShakeRandomness);
            });
        }
        else
        {
            adTweener.Complete();
        }

        if (menuCO != null)
            StopCoroutine(menuCO);

        helper.PerformWithDelay(show ? menuFadeInDelay : 0, () =>
        {
            menuCO = StartCoroutine(transitions.Fade(menuCG, menuFadeTime, show));
        });
    }

    // При нажатии на "Главное меню"
    public void OnMainMenuButtonPressed()
    {
        ScenesManager.Instance.LoadScene(0);
    }
}
