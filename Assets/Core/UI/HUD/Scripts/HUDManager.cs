using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float animationTime;
    [SerializeField] float scaleMultiplier;
    [SerializeField] Color defaultWeaponSlotColor;
    [SerializeField] Color playerTurnColor;
    [SerializeField] Color enemyTurnColor;

    [Header("References")]
    [SerializeField] TextMeshProUGUI turnSideTMP;
    [SerializeField] TextMeshProUGUI turnTMP;
    [SerializeField] TextMeshProUGUI timerTitleTMP;
    [SerializeField] TextMeshProUGUI timerTMP;
    [SerializeField] WeaponSlot[] weaponSlots;

    WeaponSlot currentWeapon;

    void Start()
    {
        currentWeapon = weaponSlots[0];
        weaponSlots[0].Select(true, scaleMultiplier, 0, defaultWeaponSlotColor);
    }

    // Выбираем оружие
    public void SelectWeapon(int index)
    {
        currentWeapon.Select(false, scaleMultiplier, animationTime, defaultWeaponSlotColor);

        currentWeapon = weaponSlots[index];

        currentWeapon.Select(true, scaleMultiplier, animationTime, defaultWeaponSlotColor);
    }

    // Обновляем текст с информацией о ходящей стороне
    public void UpdateSideTurnText(bool show, bool isPlayerTurn = false)
    {
        string side = isPlayerTurn ? "Player's" : "Enemy's";

        turnTMP.text = show ? "turn" : "";
        turnSideTMP.text = show ? side : "";
        turnSideTMP.color = isPlayerTurn ? playerTurnColor : enemyTurnColor;
    }

    // Обновляем текст с информацией и подготовке к ходу
    public void UpdatePreparationText(float time, bool show = true)
    {
        timerTitleTMP.text = show ? "preparation" : "move ends in";
        timerTMP.text = time.ToString("F0");
    }

    // Обновляем время таймера
    public void UpdateTimer(float time)
    {
        timerTMP.text = time.ToString("F0");
    }

    // Включаем/выключаем кнопки слотов оружия
    public void WeaponSlotButtonsState(bool activate)
    {
        foreach (var slot in weaponSlots)
            slot.Button.interactable = activate;
    }
}
