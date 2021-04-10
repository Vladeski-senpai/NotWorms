using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float yOffset;
    [SerializeField] ShakeSettings[] shakeSettings;

    [Header("References")]
    [SerializeField] Transform player;
    [SerializeField] Transform shakeContainer;

    public static CameraController Instance;

    Coroutine shakeCO;
    Vector3 newPosition;

    void Awake()
    {
        Instance = this;
    }

    void LateUpdate()
    {
        newPosition = player.position;
        newPosition.y += yOffset;
        transform.position = newPosition;
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
}

[System.Serializable]
public class ShakeSettings
{
    public float MinShakingDuration;
    public float MaxShakingDuration;
    public float MinShakingMagnitude;
    public float MaxShakingMagnitude;
}