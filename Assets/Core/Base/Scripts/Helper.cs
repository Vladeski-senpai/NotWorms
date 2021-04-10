using System;
using System.Collections;
using UnityEngine;

public class Helper : MonoBehaviour
{
    public static Helper Instance;

    void Awake()
    {
        Instance = this;
    }

    // Выполняем действие с задержкой
    public void PerformWithDelay(float time, Action action)
    {
        StartCoroutine(ActionWithDelay(time, action));
    }

    // Действие с задержкой
    IEnumerator ActionWithDelay(float time, Action action)
    {
        yield return new WaitForSeconds(time);

        action.Invoke();
    }
}
