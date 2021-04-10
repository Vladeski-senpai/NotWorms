using UnityEngine;

public class ButtonAnimation : MonoBehaviour
{
    [SerializeField] RectTransform rectTransform;
    [SerializeField] float scaleMultiplier;
    [SerializeField] float scaleAnimationTime;

    Transitions transitions;
    Coroutine interactionCO;
    Vector2 startScale;

    void Start()
    {
        transitions = Transitions.Instance;
        startScale = rectTransform.localScale;
    }

    // Когда зажали кнопку
    public void OnPointerDown()
    {
        if (interactionCO != null)
            StopCoroutine(interactionCO);

        interactionCO = StartCoroutine(transitions.ChangeScale(rectTransform,
            startScale, startScale * scaleMultiplier, scaleAnimationTime));
    }

    // Когда отпустили кнопку
    public void OnPointerUp()
    {
        if (interactionCO != null)
            StopCoroutine(interactionCO);

        interactionCO = StartCoroutine(transitions.ChangeScale(rectTransform,
            rectTransform.localScale, startScale, scaleAnimationTime));
    }
}
