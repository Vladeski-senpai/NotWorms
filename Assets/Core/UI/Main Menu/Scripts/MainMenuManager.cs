using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] NicknameColor[] nicknameColors;

    [Header("References")]
    [SerializeField] TextMeshProUGUI nickNameTMP;
    [SerializeField] TMP_InputField nickNameIF;
    [SerializeField] TMP_InputField rIF;
    [SerializeField] TMP_InputField gIF;
    [SerializeField] TMP_InputField bIF;
    [SerializeField] TMP_Dropdown colorsDropdown;
    [SerializeField] Button defaultColorsButton;
    [SerializeField] Button customColorButton;
    [SerializeField] TMP_InputField[] colorsIFs;

    Director director;
    bool canChangeColor;

    void Start()
    {
        director = Director.Instance;

        ColorButtonsState();
        SetUpColorsDropdown();
        SetUpIFs();

        canChangeColor = true;
    }

    // ����������� ����
    void SetUpIFs()
    {
        nickNameTMP.color = director.GameMeta.PlayerNameColor;
        nickNameTMP.text = director.GameMeta.PlayerName;

        nickNameIF.characterLimit = director.GameSettings.NickNameMaxLength;
        nickNameIF.text = director.GameMeta.PlayerName;

        colorsDropdown.value = (int)director.GameMeta.ColorID;

        rIF.text = director.GameMeta.R.ToString();
        gIF.text = director.GameMeta.G.ToString();
        bIF.text = director.GameMeta.B.ToString();
    }

    // ����������� ������ 
    void SetUpColorsDropdown()
    {
        List<string> colors = Enum.GetNames(typeof(NicknameColors)).OfType<string>().ToList();

        colorsDropdown.ClearOptions();
        colorsDropdown.AddOptions(colors);
    }

    // ������ ���� ������������� ����
    void ChangeNicknameColor(int index)
    {
        bool found = false;

        foreach (var color in nicknameColors)
            if (color.ColorID == (NicknameColors)index)
            {
                found = true;
                nickNameTMP.color = color.Color;
                director.GameMeta.PlayerNameColor = color.Color;

                break;
            }

        // ���� ����������� ��������� ����
        if (!found) Debug.LogError("Color { " + (NicknameColors)index + " } not found!");
    }

    // ��������� ������ � ����
    void ColorButtonsState()
    {
        bool customColor = director.GameMeta.CustomColor;

        defaultColorsButton.interactable = customColor;
        customColorButton.interactable = !customColor;
        colorsDropdown.interactable = !customColor;

        foreach (var button in colorsIFs)
            button.interactable = customColor;
    }

    // ��������� ����� �� ������
    byte CheckText(TMP_InputField colorIF)
    {
        bool bad = colorIF.text == "";
        int checkValue = 0;

        if (!bad) checkValue = Convert.ToInt32(colorIF.text);

        if (checkValue > 255)
        {
            checkValue = 255;
            colorIF.text = "255";
        }

        return Convert.ToByte(checkValue);
    }

    // ��� ��������� IF
    public void OnInputFieldChanged()
    {
        string nickName = nickNameIF.text;

        director.GameMeta.PlayerName = nickName;
        nickNameTMP.text = nickName;
    }

    // ��� ��������� ������ ������
    public void OnColorsDropdownChanged()
    {
        if (!canChangeColor) return;

        director.GameMeta.ColorID = (NicknameColors)colorsDropdown.value;

        ChangeNicknameColor(colorsDropdown.value);
    }

    // ��� ��������� ����� ������
    public void OnCustomColorsChanged()
    {
        if (!canChangeColor) return;

        byte r = CheckText(rIF);
        byte g = CheckText(gIF);
        byte b = CheckText(bIF);

        Color32 color = new Color32(r, g, b, 255);

        director.GameMeta.R = r;
        director.GameMeta.G = g;
        director.GameMeta.B = b;
        director.GameMeta.PlayerNameColor = color;
        nickNameTMP.color = color;
    }

    // ��� ������� �� "����� �����"
    public void OnColorModeButtonPressed(bool customColor)
    {
        if (!canChangeColor) return;

        director.GameMeta.CustomColor = customColor; // ���� true, �� ��� ��� ����� ��� ������ ���� (RGB)

        ColorButtonsState();

        if (customColor)
            OnCustomColorsChanged();
        else
            OnColorsDropdownChanged();
    }

    // ��� ������� �� "������"
    public void OnPlayButtonPressed()
    {
        ScenesManager.Instance.LoadScene(1);
    }
}

[Serializable]
public class NicknameColor
{
    public NicknameColors ColorID;
    public Color Color;
}
