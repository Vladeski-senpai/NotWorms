using UnityEngine;
using UnityEngine.Events;

public class GameOverManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float menuFadeTime;
    [SerializeField] float menuFadeInDelay;

    [Header("References")]
    [SerializeField] CanvasGroup menuCG;

    [Header("Events")]
    [SerializeField] UnityEvent OnAdButtonPressedEvent;

    Transitions transitions;
    Coroutine menuCO;
    Helper helper;

    void Start()
    {
        transitions = Transitions.Instance;
        helper = Helper.Instance;
    }

    // �������� ��������� ����
    public void MenuState(bool show)
    {
        menuCG.interactable = show;
        menuCG.blocksRaycasts = show;

        if (show) Cursor.visible = true;

        if (menuCO != null)
            StopCoroutine(menuCO);

        helper.PerformWithDelay(show ? menuFadeInDelay : 0, () =>
        {
            menuCO = StartCoroutine(transitions.Fade(menuCG, menuFadeTime, show));
        });
    }

    // ��� ������� �� "������� ����"
    public void OnMainMenuButtonPressed()
    {
        ScenesManager.Instance.LoadScene(0);
    }

    // ��� ������� �� "���������� ����"
    public void OnAdButtonPressed()
    {
        MenuState(false);
        OnAdButtonPressedEvent.Invoke();
    }
}
