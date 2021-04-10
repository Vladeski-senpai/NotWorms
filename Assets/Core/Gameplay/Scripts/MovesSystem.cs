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

    // Начинаем ход
    void StartMove()
    {
        // Ход ботов
        if (!IsPlayerTurn)
        {
            bool botFound = false;

            // Ищем бота который может ходить
            foreach (var bot in gameManager.BotManagers)
            {
                if (bot == null || bot.MoveWasMade) continue;

                bot.MoveWasMade = true;
                botFound = bot.TurnState(true);

                if (botFound) botManager = bot;

                break;
            }

            // Если все боты сделали свой ход
            if (!botFound)
            {
                IsPlayerTurn = true;
                StartMove();

                return;
            }
        }
        // Ход игрока
        else playerManager.TurnState(true);

        StartTimer();
    }

    // Заканчиваем ход
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

        // Начинаем ход заново
        helper.PerformWithDelay(director.GameSettings.TurnSwitchDelay, () =>
        {
            StartMove();
        });
    }

    // Сбрасываем ходы всех ботов
    void ClearBots()
    {
        foreach (var bot in gameManager.BotManagers)
            bot.MoveWasMade = false;
    }

    // Запускаем таймер хода
    void StartTimer()
    {
        StopTimer();

        timerCO = StartCoroutine(Timer());
    }

    // Останавливаем таймер
    void StopTimer()
    {
        if (timerCO != null)
            StopCoroutine(timerCO);
    }

    // Таймер хода
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
