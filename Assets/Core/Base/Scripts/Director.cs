using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] GameSettings gameSettings;

    [Header("Meta")]
    [SerializeField] GameMeta gameMeta;

    public static Director Instance;

    // Settings
    public GameSettings GameSettings => gameSettings;

    // Meta
    public GameMeta GameMeta => gameMeta;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else Destroy(gameObject);
    }
}
