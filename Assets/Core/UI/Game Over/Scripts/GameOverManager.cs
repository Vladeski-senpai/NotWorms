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
    [SerializeField] GameObject lostTitle;
    [SerializeField] GameObject wonTitle;
    [SerializeField] GameObject adButton;

    Tweener menuTweener;
    Tweener adTweener;

    public void GameResult(bool lost)
    {
        lostTitle.SetActive(lost);
        wonTitle.SetActive(!lost);
        adButton.SetActive(lost);
    }

    // Включаем выключаем меню
    public void MenuState(bool show)
    {
        menuCG.interactable = show;
        menuCG.blocksRaycasts = show;

        if (show)
            adTweener = adButtonRT.DOShakeAnchorPos(adBtnShakeDuration, adBtnShakeVector, adBtnShakeVibrato, adBtnShakeRandomness).SetDelay(adBtnShakeDelay);
        else
            adTweener.Complete();

        menuTweener.Complete();
        menuTweener = menuCG.DOFade(show ? 1 : 0, menuFadeTime).SetDelay(show ? menuFadeInDelay : 0);
    }

    // При нажатии на "Главное меню"
    public void OnMainMenuButtonPressed()
    {
        ScenesManager.Instance.LoadScene(0);
    }
}
