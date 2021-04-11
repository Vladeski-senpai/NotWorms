using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class Director : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] GameSettings gameSettings;
    [SerializeField] PlayerSettings playerSettings;

    [Header("Meta")]
    [SerializeField] GameMeta gameMeta;

    public static Director Instance;

    // Settings
    public GameSettings GameSettings => gameSettings;
    public PlayerSettings PlayerSettings => playerSettings;

    // Meta
    public GameMeta GameMeta => gameMeta;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(this);
            //MobileAds.Initialize(initst);
        }
        else Destroy(gameObject);
    }
}
