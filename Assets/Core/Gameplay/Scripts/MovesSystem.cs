using System.Collections;
using TMPro;
using UnityEngine;

public class MovesSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameManager gameManager;
    [SerializeField] HUDManager hudManager;
    [SerializeField] CameraController cameraController;

    public bool IsPlayerTurn { get; private set; }

    PlayerManager playerManager;
    BotManager botManager;
    Coroutine preparationCO;
    Coroutine timerCO;
    Director director;
    float currentTime;
    float moveTime;
    bool isGameFinished;

    public void Init()
    {
        director = Director.Instance;
        playerManager = gameManager.PlayerManager;
        moveTime = director.GameSettings.MoveTime;
        IsPlayerTurn = true;

        hudManager.UpdateSideTurnText(false);
    }

    // �������� ���
    public void StartMove(float delay = 0)
    {
        if (isGameFinished) return;

        if (preparationCO != null)
            StopCoroutine(preparationCO);

        preparationCO = StartCoroutine(PreparationTimer(delay));
    }

    void Move()
    {
        // ��� �����
        if (!IsPlayerTurn)
        {
            bool botFound = false;

            // ���� ���� ������� ����� ������
            foreach (var bot in gameManager.BotManagers)
            {
                if (bot == null || bot.MoveWasMade) continue;

                bot.MoveWasMade = true;
                botFound = bot.TurnState(true);

                // ���� ��� ����� ������
                if (botFound)
                {
                    botManager = bot;
                    hudManager.UpdateSideTurnText(true);
                    hudManager.WeaponSlotButtonsState(false);
                    cameraController.ChangeTarget(bot.transform);
                }

                break;
            }

            // ���� ��� ���� ������� ���� ���
            if (!botFound)
            {
                IsPlayerTurn = true;
                StartMove();

                return;
            }
        }
        // ��� ������
        else
        {
            hudManager.UpdateSideTurnText(true, true);
            hudManager.WeaponSlotButtonsState(true);
            playerManager.TurnState(true);
            cameraController.ChangeTarget(playerManager.transform);
        }

        StartTimer();
    }

    // ����������� ���
    public void StopMove()
    {
        StopTimer();

        if (IsPlayerTurn)
        {
            playerManager.TurnState(false);

            IsPlayerTurn = false;
            ClearBots();
        }
        else if (botManager != null) botManager.TurnState(false);

        // �������� ��� ������
        StartMove(director.GameSettings.TurnSwitchDelay);
    }

    // �������� ������������� ����
    public void FinishGame()
    {
        isGameFinished = true;

        StopTimer();
        ClearBots();
        hudManager.UpdateTimer(0);
        hudManager.UpdateSideTurnText(false);

        if (!IsPlayerTurn && botManager != null) botManager.TurnState(false);
        else playerManager.TurnState(false);

        IsPlayerTurn = true;
    }

    // ������������� ����
    public void RestartGame(float delay = 0)
    {
        isGameFinished = false;

        StartMove(delay);
    }

    // ���������� ���� ���� �����
    void ClearBots()
    {
        foreach (var bot in gameManager.BotManagers)
            bot.MoveWasMade = false;
    }

    // ��������� ������ ����
    void StartTimer()
    {
        StopTimer();

        timerCO = StartCoroutine(Timer());
    }

    // ������������� ������
    public void StopTimer()
    {
        if (timerCO != null)
            StopCoroutine(timerCO);

        if (preparationCO != null)
            StopCoroutine(preparationCO);
    }

    // ������ ����
    IEnumerator Timer()
    {
        float startValue = moveTime + 0.49f; // TEMP
        float elapsedTime = 0;

        while (elapsedTime < moveTime)
        {
            elapsedTime += Time.deltaTime;
            currentTime = Mathf.Lerp(startValue, 0, elapsedTime / moveTime);

            hudManager.UpdateTimer(currentTime);

            yield return null;
        }

        currentTime = 0;

        hudManager.UpdateTimer(0);
        StopMove();
    }

    // ���������� ����� �����
    IEnumerator PreparationTimer(float time)
    {
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            hudManager.UpdatePreparationText(Mathf.Lerp(time, 0, elapsedTime / time));

            yield return null;
        }

        hudManager.UpdatePreparationText(0, false);

        yield return new WaitForSeconds(0.5f);

        Move();
    }
}
