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

    // Metas
    public WeaponsMeta WeaponsMeta => weaponsMeta;

    // Managers
    public GameOverManager GameOverManager => gameOverManager;
    public HUDManager HUDManager => hudManager;
    public PlayerManager PlayerManager => playerManager;
    public MovesSystem MovesSystem => movesSystem;

    // Other
    public List<BotManager> BotManagers { get; private set; }

    public static GameManager Instance;

    void Awake()
    {
        Instance = this;
        BotManagers = new List<BotManager>();
    }

    // ���������� ������
    public void ResetPlayer()
    {
        playerManager.RespawnPlayer();
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
}
