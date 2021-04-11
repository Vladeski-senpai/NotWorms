using UnityEngine;

[System.Serializable]
public class GameSettings
{
    [Header("Game settings")]
    [SerializeField] float moveTime;
    [Tooltip("������������ ������ ���� ������")]
    [SerializeField] int nickNameMaxLength;
    [SerializeField] float roundStartDelay;
    [SerializeField] float turnSwitchDelay;

    [Header("Camera settings")]
    [SerializeField] float cameraAimTime;
    [SerializeField] float yOffset;

    [Header("Bots settins")]
    [SerializeField] string[] botNames;


    // Camera settings
    public float CameraAimTime => cameraAimTime;
    public float YOffset => yOffset;

    // Game settings
    public float MoveTime => moveTime;
    public int NickNameMaxLength => nickNameMaxLength;
    public float RoundStartDelay => roundStartDelay;
    public float TurnSwitchDelay => turnSwitchDelay;

    // Bots settings
    public string[] BotNames => botNames;
}

[System.Serializable]
public class PlayerSettings
{
    [Header("Player settings")]
    [SerializeField] float startHealth;
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpStrength;
    [SerializeField] float jumpTimer;
    [SerializeField] float aimHoldTimer;

    [Header("Player's aim settings")]
    [SerializeField] float aimDistance;
    [SerializeField] float launchSpeed;
    [SerializeField] float shellGravityScale;
    [SerializeField] float trajectoryTimeMultilpier;
    [SerializeField] int pointsCount;
    [SerializeField] Color trajectoryColor;


    // Player settings
    public float StartHealth => startHealth;
    public float MoveSpeed => moveSpeed;
    public float JumpStrength => jumpStrength;
    public float JumpTimer => jumpTimer;
    public float AimHoldTimer => aimHoldTimer;

    // Player's aim settings
    public float AimDistance => aimDistance;
    public float LaunchSpeed => launchSpeed;
    public float ShellGravityScale => shellGravityScale;
    public float TrajectoryTimeMultilpier => trajectoryTimeMultilpier;
    public int PointsCount => pointsCount;
    public Color TrajectoryColor => trajectoryColor;
}

[System.Serializable]
public class BotsSettings
{
    [Header("Bots settings")]
    [SerializeField] float startHealth;
    [SerializeField] float damage;
    [SerializeField] float moveSpeed;
    [SerializeField] float minAccuracy;
    [SerializeField] float maxAccuracy;
    [SerializeField] float minShootDistance;
    [SerializeField] float maxShootDistance;
    [SerializeField] float shootPointDestroyDistance;
    [SerializeField] float minShootHeight;
    [SerializeField] float maxShootHeight;


    // Bots settings
    public float StartHealth => startHealth;
    public float Damage => damage;
    public float MoveSpeed => moveSpeed;
    public float MinAccuracy => minAccuracy;
    public float MaxAccuracy => maxAccuracy;
    public float MinShootDistance => minShootDistance;
    public float MaxShootDistance => maxShootDistance;
    public float ShootPointDestroyDistance => shootPointDestroyDistance;
    public float MinShootHeight => minShootHeight;
    public float MaxShootHeight => maxShootHeight;
}