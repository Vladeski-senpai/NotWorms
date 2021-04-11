using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAnimation : MonoBehaviour
{
    [SerializeField] RectTransform rectTransform;
    [SerializeField] float scaleMultiplier;
    [SerializeField] float scaleAnimationTime;
    [SerializeField] bool checkInteractable;

    Tweener buttonTweener;
    Button button;
    Vector2 startScale;
    bool isInteractable;

    void Start()
    {
        startScale = rectTransform.localScale;
        isInteractable = true;

        if (checkInteractable) button = GetComponent<Button>();
    }

    // Когда зажали кнопку
    public void OnPointerDown()
    {
        if (checkInteractable)
            isInteractable = button.interactable;

        if (!isInteractable) return;

        buttonTweener.Complete();
        buttonTweener = rectTransform.DOScale(startScale * scaleMultiplier, scaleAnimationTime);
    }

    // Когда отпустили кнопку
    public void OnPointerUp()
    {
        if (checkInteractable)
            isInteractable = button.interactable;

        if (!isInteractable) return;

        buttonTweener.Complete();
        buttonTweener = rectTransform.DOScale(startScale, scaleAnimationTime);
    }
}
