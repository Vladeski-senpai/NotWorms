using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("TEMPORARY")]
    [SerializeField] bool startMatch;

    [Header("Metas")]
    [SerializeField] WeaponsMeta weaponsMeta;

    [Header("Managers / etc.")]
    [SerializeField] GameOverManager gameOverManager;
    [SerializeField] HUDManager hudManager;
    [SerializeField] PlayerManager playerManager;
    [SerializeField] MovesSystem movesSystem;
    [SerializeField] CameraController cameraController;

    // Metas
    public WeaponsMeta WeaponsMeta => weaponsMeta;

    // Managers
    public PlayerManager PlayerManager => playerManager;
    public MovesSystem MovesSystem => movesSystem;
    public CameraController CameraController => cameraController;

    // Other
    public List<BotManager> BotManagers { get; private set; }

    public static GameManager Instance;

    Director director;

    void Awake()
    {
        Instance = this;
        BotManagers = new List<BotManager>();
    }

    void Start()
    {
        director = Director.Instance;

        if (startMatch) movesSystem.StartMove(director.GameSettings.RoundStartDelay);
    }

    // Добавляем бота в список ботов
    public void RegisterBot(BotManager botManager)
    {
        BotManagers.Add(botManager);
    }

    // Удаляем бота из списка ботов
    public void DeleteBot(BotManager botManager)
    {
        BotManagers.Remove(botManager);
    }

    // Когда кто-то закончил ход
    public void OnMoveEnded()
    {
        MovesSystem.StopMove();
    }

    // При проигрыше
    public void GameOver()
    {
        movesSystem.FinishGame();
        gameOverManager.MenuState(true);
    }

    // При нажатии на "Продолжить игру"
    public void OnContinueButtonPressed()
    {
        playerManager.RespawnPlayer();
        movesSystem.RestartGame(director.GameSettings.TurnSwitchDelay);
    }
}
