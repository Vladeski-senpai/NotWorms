using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] ShakeSettings[] shakeSettings;

    [Header("References")]
    [SerializeField] Transform shakeContainer;

    public static CameraController Instance;

    GameSettings gameSettings;
    Coroutine cameraAimCO;
    Coroutine shakeCO;
    Transform target;
    Vector3 newPosition;
    bool isActive;
    bool canMove;

    void Awake()
    {
        Instance = this;
    }

    public void Init(GameSettings _gameSettings)
    {
        gameSettings = _gameSettings;
    }

    void LateUpdate()
    {
        FollowTarget();
    }

    // Преследуем цель
    void FollowTarget()
    {
        if (!isActive || target == null) return;

        newPosition = target.position;
        newPosition.y += gameSettings.YOffset;

        if (canMove) transform.position = newPosition;
    }

    // Меняем цель преследования
    public void ChangeTarget(Transform _target)
    {
        target = _target;

        if (!isActive) isActive = true;

        if (cameraAimCO != null)
            StopCoroutine(cameraAimCO);

        // Плавно наводим камеру на цель
        cameraAimCO = StartCoroutine(AimOnTarget());

        // Не обновляет динамически позицию
        //transform.DOMove(newPosition, cameraAimTime).OnComplete(() => { canMove = true; });
    }

    // Трясём камеру
    public void Shake(int shakeIndex)
    {
        if (shakeCO != null)
            StopCoroutine(shakeCO);

        shakeCO = StartCoroutine(Shaking(shakeIndex));
    }

    // Тряска камеры
    IEnumerator Shaking(int index)
    {
        float duration = Random.Range(shakeSettings[index].MinShakingDuration, shakeSettings[index].MaxShakingDuration);
        float magnitude = Random.Range(shakeSettings[index].MinShakingMagnitude, shakeSettings[index].MaxShakingMagnitude);
        float elapsedTime = 0;

        shakeContainer.localPosition = Vector2.zero;

        while (elapsedTime < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            shakeContainer.localPosition = new Vector2(x, y);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        shakeContainer.localPosition = Vector2.zero;
    }

    // Наводим камеру на цель
    IEnumerator AimOnTarget()
    {
        Vector2 startPosition = transform.position;
        float elapsedTime = 0;

        canMove = false;

        while (elapsedTime < gameSettings.CameraAimTime)
        {
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, newPosition, elapsedTime / gameSettings.CameraAimTime);

            yield return null;
        }

        transform.position = newPosition;
        canMove = true;
    }
}

[System.Serializable]
public class ShakeSettings
{
    public float MinShakingDuration;
    public float MaxShakingDuration;
    public float MinShakingMagnitude;
    public float MaxShakingMagnitude;
}