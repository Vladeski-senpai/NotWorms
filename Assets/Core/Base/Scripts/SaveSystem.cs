using System;
using UnityEngine;

public static class SaveSystem
{
    public static void Save()
    {
        GameMeta gameMeta = Director.Instance.GameMeta;

        PlayerPrefs.SetString("nickname", gameMeta.PlayerName);
        PlayerPrefs.SetInt("defaultCursor", gameMeta.DefaultCursor ? 0 : 1);
        PlayerPrefs.SetInt("customColor", gameMeta.CustomColor ? 1 : 0);
        PlayerPrefs.SetInt("colorID", (int)gameMeta.ColorID);
        PlayerPrefs.SetInt("r", gameMeta.R);
        PlayerPrefs.SetInt("g", gameMeta.G);
        PlayerPrefs.SetInt("b", gameMeta.B);
    }

    public static SaveData Load()
    {
        SaveData saveData = new SaveData
        {
            nickname = PlayerPrefs.GetString("nickname"),
            defaultCursor = PlayerPrefs.GetInt("defaultCursor") == 0,
            customColor = PlayerPrefs.GetInt("customColor") == 1,
            colorID = (NicknameColors)PlayerPrefs.GetInt("colorID"),
            r = Convert.ToByte(PlayerPrefs.GetInt("r")),
            g = Convert.ToByte(PlayerPrefs.GetInt("g")),
            b = Convert.ToByte(PlayerPrefs.GetInt("b"))
        };

        return saveData;
    }
}

public class SaveData
{
    public string nickname { get; set; }
    public bool defaultCursor { get; set; }
    public bool customColor { get; set; }
    public NicknameColors colorID { get; set; }
    public byte r { get; set; }
    public byte g { get; set; }
    public byte b { get; set; }
}