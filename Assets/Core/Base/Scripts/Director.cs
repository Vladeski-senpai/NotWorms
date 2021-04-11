using UnityEngine;

public class Director : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] GameSettings gameSettings;
    [SerializeField] PlayerSettings playerSettings;
    [SerializeField] BotsSettings botsSettings;

    [Header("Meta")]
    [SerializeField] GameMeta gameMeta;

    public static Director Instance;

    // Settings
    public GameSettings GameSettings => gameSettings;
    public PlayerSettings PlayerSettings => playerSettings;
    public BotsSettings BotsSettings => botsSettings;

    // Meta
    public GameMeta GameMeta => gameMeta;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(this);
            LoadData();
        }
        else Destroy(gameObject);
    }

    // Загружаем данные
    void LoadData()
    {
        SaveData saveData = SaveSystem.Load();

        gameMeta.PlayerName = saveData.nickname;
        gameMeta.DefaultCursor = saveData.defaultCursor;
        gameMeta.CustomColor = saveData.customColor;
        gameMeta.ColorID = saveData.colorID;
        gameMeta.R = saveData.r;
        gameMeta.G = saveData.g;
        gameMeta.B = saveData.b;
    }
}
