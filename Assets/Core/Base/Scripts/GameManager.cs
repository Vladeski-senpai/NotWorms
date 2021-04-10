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

    // Сбрасываем игрока
    public void ResetPlayer()
    {
        playerManager.RespawnPlayer();
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
}
