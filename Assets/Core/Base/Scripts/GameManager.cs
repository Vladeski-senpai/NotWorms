using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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
    public HUDManager HUDManager => hudManager;
    public PlayerManager PlayerManager => playerManager;
    public MovesSystem MovesSystem => movesSystem;

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
        SoundManager.Instance.Play(SoundName.Wind);

        InitializeManagers();

        movesSystem.StartMove(director.GameSettings.RoundStartDelay);
    }

    void InitializeManagers()
    {
        hudManager.Init();
        cameraController.Init(director.GameSettings);
        movesSystem.Init();
    }

    // ��������� ���� � ������ �����
    public void RegisterBot(BotManager botManager)
    {
        BotManagers.Add(botManager);
    }

    // ������� ���� �� ������ �����
    public void DeleteBot(BotManager botManager)
    {
        BotManagers.Remove(botManager);
    }

    // ����� ���-�� �������� ���
    public void OnMoveEnded()
    {
        MovesSystem.StopMove();
    }

    // ��� ���������
    public void GameOver()
    {
        movesSystem.FinishGame();
        gameOverManager.MenuState(true);
    }

    // ��� ������� �� "���������� ����"
    public void OnContinueButtonPressed()
    {
        playerManager.RespawnPlayer();
        movesSystem.RestartGame(director.GameSettings.TurnSwitchDelay);
    }
}
