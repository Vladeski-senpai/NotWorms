using UnityEngine;

[System.Serializable]
public class GameSettings
{
    [Header("Game settings")]
    [Tooltip("Максимальная длинна ника игрока")]
    [SerializeField] int nickNameMaxLength;
    [SerializeField] float roundStartDelay;
    [SerializeField] float turnSwitchDelay;

    [Header("Bots settins")]
    [SerializeField] string[] botNames;


    // Game settings
    public int NickNameMaxLength => nickNameMaxLength;
    public float RoundStartDelay => roundStartDelay;
    public float TurnSwitchDelay => turnSwitchDelay;

    // Bots settings
    public string[] BotNames => botNames;
}

[System.Serializable]
public class PlayerSettings
{
    [SerializeField] float startHealth;
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpStrength;
    [SerializeField] float jumpTimer;
    [SerializeField] float aimHoldTimer;


    public float StartHealth => startHealth;
    public float MoveSpeed => moveSpeed;
    public float JumpStrength => jumpStrength;
    public float JumpTimer => jumpTimer;
    public float AimHoldTimer => aimHoldTimer;
}