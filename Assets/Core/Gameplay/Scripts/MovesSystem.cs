using System.Collections;
using TMPro;
using UnityEngine;

public class MovesSystem : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float moveTime;

    [Header("References")]
    [SerializeField] GameManager gameManager;
    [SerializeField] TextMeshProUGUI timerTMP;

    public bool IsPlayerTurn { get; private set; }

    PlayerManager playerManager;
    BotManager botManager;
    Coroutine timerCO;
    Director director;
    Helper helper;
    float currentTime;

    void Start()
    {
        director = Director.Instance;
        helper = Helper.Instance;
        playerManager = gameManager.PlayerManager;
        IsPlayerTurn = true;

        helper.PerformWithDelay(director.GameSettings.RoundStartDelay, () =>
        {
            StartMove();
        });
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            StartTimer();
    }

    // �������� ���
    void StartMove()
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

                if (botFound) botManager = bot;

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
        else playerManager.TurnState(true);

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
        helper.PerformWithDelay(director.GameSettings.TurnSwitchDelay, () =>
        {
            StartMove();
        });
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
    void StopTimer()
    {
        if (timerCO != null)
            StopCoroutine(timerCO);
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
            timerTMP.text = currentTime.ToString("F0");

            yield return null;
        }

        currentTime = 0;
        timerTMP.text = "0";

        StopMove();
    }
}
