using DG.Tweening;
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

    Tweener scaleTweener;
    Tweener colorTweener;
    Vector2 startScale;

    void Awake()
    {
        startScale = rectTransform.localScale;
    }

    // �������� ������
    public void Select(bool select, float scaleMultiplier, float duration, Color color)
    {
        outline.color = select ? selectedColor : color;

        // ������ �����
        scaleTweener.Complete();
        scaleTweener = rectTransform.DOScale(select ? startScale * scaleMultiplier : startScale, duration);

        // ������ ����
        colorTweener.Complete();
        colorTweener = outline.DOColor(select ? selectedColor : color, duration);

        /*select ? color : selectedColor, select ? selectedColor : color*/
    }
}
