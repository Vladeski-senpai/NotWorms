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

    // ��������� �������� � ���������
    public void PerformWithDelay(float time, Action action)
    {
        StartCoroutine(ActionWithDelay(time, action));
    }

    // �������� � ���������
    IEnumerator ActionWithDelay(float time, Action action)
    {
        yield return new WaitForSeconds(time);

        action.Invoke();
    }
}
