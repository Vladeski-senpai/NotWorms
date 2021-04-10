using UnityEngine;

public class Aim : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float aimDistance;
    [SerializeField] float launchSpeed;
    [SerializeField] float shellLinearDrag;
    [SerializeField] float shellGravityScale;
    [SerializeField] float trajectoryTimeMultilpier;
    [SerializeField] int pointsCount;
    [SerializeField] bool showTrajectory;

    [Header("References")]
    [SerializeField] Camera cam;
    [SerializeField] Transform crosshair;
    [SerializeField] Transform spawnPoint;
    [SerializeField] Transform pointsContainer;
    [SerializeField] GameObject trajectoryPoint;

    // Other
    public Transform SpawnPoint => spawnPoint;
    public Vector3 CursorPosition => cursorPosition;
    public float LaunchSpeed => launchSpeed;
    public float ShellLinearDrag => shellLinearDrag;
    public float ShellGravityScale => shellGravityScale;

    GameObject[] points;
    Vector3 cursorPosition;
    Vector3 lookDirection;
    float lookAngle;

    void Start()
    {
        points = new GameObject[pointsCount];

        for (int i = 0; i < pointsCount; i++)
            points[i] = Instantiate(trajectoryPoint, transform.position, Quaternion.identity, pointsContainer);

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
        cursorPosition = cam.ScreenToWorldPoint(Input.mousePosition);
        cursorPosition.z = 0;
    }

    // Двигаем прицел
    void MoveCrosshair()
    {
        lookDirection = cursorPosition - transform.position;
        //lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        //crosshair.rotation = Quaternion.AngleAxis(lookAngle, Vector3.forward);
        crosshair.localPosition = Vector2.ClampMagnitude(lookDirection.normalized * 10, aimDistance);
    }

    // Обновляем траекторию снаряда
    public void UpdateTrajectory()
    {
        Vector2 lookDirection = (cursorPosition - transform.position).normalized;

        for (int i = 0; i < pointsCount; i++)
            points[i].transform.position = PointPosition(lookDirection, i * trajectoryTimeMultilpier);
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
        Vector2 currentPosition = (Vector2)spawnPoint.position + (lookDirection * launchSpeed * t) +
            0.5f * (Physics2D.gravity * shellGravityScale) * (t * t);

        return currentPosition;
    }
}
