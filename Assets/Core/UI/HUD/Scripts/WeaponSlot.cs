using UnityEngine;
using UnityEngine.UI;

public class WeaponSlot : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] Color selectedColor;

    [Header("References")]
    [SerializeField] RectTransform rectTransform;
    [SerializeField] Image outline;
    [SerializeField] Button button;

    public Button Button => button;

    Transitions transitions;
    Coroutine scaleCO;
    Coroutine colorCO;
    Vector2 startScale;
    bool hasTransitionComponent;

    void Awake()
    {
        startScale = rectTransform.localScale;
    }

    // Выбираем оружие
    public void Select(bool select, float scaleMultiplier, float duration, Color color)
    {
        if (!hasTransitionComponent)
        {
            transitions = Transitions.Instance;
            hasTransitionComponent = true;
        }

        outline.color = select ? selectedColor : color;

        if (scaleCO != null)
        {
            StopCoroutine(scaleCO);
            StopCoroutine(colorCO);
        }

        // Меняем скейл
        scaleCO = StartCoroutine(select ? transitions.ChangeScale(rectTransform, startScale, scaleMultiplier, duration) :
            transitions.ChangeScale(rectTransform, startScale * scaleMultiplier, startScale, duration));

        // Меняем цвет
        colorCO = StartCoroutine(transitions.LerpColor(outline, select ? color : selectedColor,
            select ? selectedColor : color, duration));
    }
}
