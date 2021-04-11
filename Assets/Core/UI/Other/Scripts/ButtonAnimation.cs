using UnityEngine;
using UnityEngine.UI;

public class ButtonAnimation : MonoBehaviour
{
    [SerializeField] RectTransform rectTransform;
    [SerializeField] float scaleMultiplier;
    [SerializeField] float scaleAnimationTime;
    [SerializeField] bool checkInteractable;

    Transitions transitions;
    Coroutine interactionCO;
    Button button;
    Vector2 startScale;
    bool isInteractable;

    void Start()
    {
        transitions = Transitions.Instance;
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

        if (interactionCO != null)
            StopCoroutine(interactionCO);

        interactionCO = StartCoroutine(transitions.ChangeScale(rectTransform,
            startScale, startScale * scaleMultiplier, scaleAnimationTime));
    }

    // Когда отпустили кнопку
    public void OnPointerUp()
    {
        if (checkInteractable)
            isInteractable = button.interactable;

        if (!isInteractable) return;

        if (interactionCO != null)
            StopCoroutine(interactionCO);

        interactionCO = StartCoroutine(transitions.ChangeScale(rectTransform,
            rectTransform.localScale, startScale, scaleAnimationTime));
    }
}
