using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameMeta", menuName = "Meta/GameMeta", order = 2)]
public class GameMeta : ScriptableObject
{
    [Header("Game")]
    [SerializeField] string playerName;
    [SerializeField] Color playerNameColor;

    [Header("Main Menu")]
    [SerializeField] NicknameColors colorID;
    [SerializeField] byte r;
    [SerializeField] byte g;
    [SerializeField] byte b;
    [SerializeField] bool customColor;


    // Game
    public string PlayerName { get => playerName; set => playerName = value; }
    public Color PlayerNameColor { get => playerNameColor; set => playerNameColor = value; }

    // Main Menu
    public NicknameColors ColorID { get => colorID; set => colorID = value; }
    public byte R { get => r; set => r = value; }
    public byte G { get => g; set => g = value; }
    public byte B { get => b; set => b = value; }
    public bool CustomColor { get => customColor; set => customColor = value; }
}
