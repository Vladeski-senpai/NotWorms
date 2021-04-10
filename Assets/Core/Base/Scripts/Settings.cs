using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSettings
{
    [Header("Game settings")]
    [Tooltip("Максимальная длинна ника игрока")]
    [SerializeField] int nickNameMaxLength;
    [SerializeField] float roundStartDelay;
    [SerializeField] float turnSwitchDelay;

    [Header("Player settings")]
    [SerializeField] float startHealth;
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpStrength;

    [Header("Bots settins")]
    [SerializeField] string[] botNames;


    // Game settings
    public int NickNameMaxLength => nickNameMaxLength;
    public float RoundStartDelay => roundStartDelay;
    public float TurnSwitchDelay => turnSwitchDelay;

    // Bots settings
    public string[] BotNames => botNames;
}
