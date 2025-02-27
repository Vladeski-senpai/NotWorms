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
    public Vector3 LookDirection => lookDirection;
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
        //GetCursorPosition();
        MoveCrosshair();
    }

    // ������� �������
    void GetCursorPosition()
    {
        cursorPosition = cam.ScreenToWorldPoint(playerManager.TouchPosition);
        cursorPosition.z = 0;
    }

    // ������� ������
    void MoveCrosshair()
    {
        lookDirection = transform.position - cursorPosition;
        //lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        //crosshair.rotation = Quaternion.AngleAxis(lookAngle, Vector3.forward);
        crosshair.localPosition = Vector2.ClampMagnitude(lookDirection.normalized * 10, playerSettings.AimDistance);
    }

    public void CheckMoveDirection()
    {
        GetCursorPosition();

        if (Mathf.Abs(lookDirection.x) > 0.1f)
            MoveDirection = cursorPosition.x > transform.position.x ? 1 : -1;
        else
            MoveDirection = 0;
    }

    // ��������� ���������� �������
    public void UpdateTrajectory()
    {
        if (!showTrajectory) return;

        Vector2 lookDirection = (transform.position - cursorPosition).normalized;

        for (int i = 0; i < playerSettings.PointsCount; i++)
            points[i].transform.position = PointPosition(lookDirection, i * playerSettings.TrajectoryTimeMultilpier);
    }

    // ��������/��������� ����� � ����������
    public void PointsState(bool show)
    {
        if (show && !showTrajectory) return;

        foreach (var point in points)
            point.SetActive(show);
    }

    // ������� ������� ��� ����� � ����������
    Vector2 PointPosition(Vector2 lookDirection, float t)
    {
        Vector2 currentPosition = (Vector2)spawnPoint.position + (lookDirection * playerSettings.LaunchSpeed * t) +
            0.5f * (Physics2D.gravity * playerSettings.ShellGravityScale) * (t * t);

        return currentPosition;
    }
}
