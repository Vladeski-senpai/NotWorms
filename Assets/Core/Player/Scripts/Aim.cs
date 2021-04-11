using UnityEngine;

public class Aim : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Camera cam;
    [SerializeField] Transform crosshair;
    [SerializeField] Transform spawnPoint;
    [SerializeField] Transform pointsContainer;
    [SerializeField] GameObject trajectoryPoint;
    [SerializeField] Transform target;

    // Other
    public Transform SpawnPoint => spawnPoint;
    public Vector3 CursorPosition => cursorPosition;
    public float LaunchSpeed => playerSettings.LaunchSpeed;
    public float ShellGravityScale => playerSettings.ShellGravityScale;
    public int MoveDirection { get; set; }

    GameObject[] points;
    PlayerSettings playerSettings;
    PlayerManager playerManager;
    Vector3 cursorPosition;
    Vector3 lookDirection;
    bool showTrajectory;
    bool isAndroid;

    void Start()
    {
        playerSettings = Director.Instance.PlayerSettings;
        showTrajectory = !Director.Instance.GameMeta.DefaultCursor;
        playerManager = GetComponent<PlayerManager>();

#if UNITY_ANDROID
        isAndroid = true;
#endif

        if (!showTrajectory) return;

        points = new GameObject[playerSettings.PointsCount];

        for (int i = 0; i < playerSettings.PointsCount; i++)
        {
            points[i] = Instantiate(trajectoryPoint, transform.position, Quaternion.identity, pointsContainer);
            points[i].GetComponent<SpriteRenderer>().color = playerSettings.TrajectoryColor;
        }

        PointsState(false);
    }

    void Update()
    {
        GetCursorPosition();
        MoveCrosshair();
    }

    // Позиция курсора
    void GetCursorPosition()
    {
        cursorPosition = cam.ScreenToWorldPoint(isAndroid ? playerManager.TouchPosition : (Vector2)Input.mousePosition);
        cursorPosition.z = 0;
    }

    // Двигаем прицел
    void MoveCrosshair()
    {
        lookDirection = cursorPosition - transform.position;
        //lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        //crosshair.rotation = Quaternion.AngleAxis(lookAngle, Vector3.forward);
        crosshair.localPosition = Vector2.ClampMagnitude(lookDirection.normalized * 10, playerSettings.AimDistance);
    }

    public void CheckMoveDirection()
    {
        MoveDirection = cursorPosition.x > transform.position.x ? 1 : -1;
    }

    // Обновляем траекторию снаряда
    public void UpdateTrajectory()
    {
        if (!showTrajectory) return;

        Vector2 lookDirection = (cursorPosition - transform.position).normalized;

        for (int i = 0; i < playerSettings.PointsCount; i++)
            points[i].transform.position = PointPosition(lookDirection, i * playerSettings.TrajectoryTimeMultilpier);
    }

    // Включаем/выключаем точки в траектории
    public void PointsState(bool show)
    {
        if (show && !showTrajectory) return;

        foreach (var point in points)
            point.SetActive(show);
    }

    // Находим позицию для точки в траектории
    Vector2 PointPosition(Vector2 lookDirection, float t)
    {
        Vector2 currentPosition = (Vector2)spawnPoint.position + (lookDirection * playerSettings.LaunchSpeed * t) +
            0.5f * (Physics2D.gravity * playerSettings.ShellGravityScale) * (t * t);

        return currentPosition;
    }
}
