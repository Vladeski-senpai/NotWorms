using TMPro;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float animationTime;
    [SerializeField] float scaleMultiplier;
    [SerializeField] float hitFlashFadeTime;
    [SerializeField] Color defaultWeaponSlotColor;
    [SerializeField] Color playerTurnColor;
    [SerializeField] Color enemyTurnColor;

    [Header("References")]
    [SerializeField] TextMeshProUGUI turnSideTMP;
    [SerializeField] TextMeshProUGUI turnTMP;
    [SerializeField] TextMeshProUGUI timerTitleTMP;
    [SerializeField] TextMeshProUGUI timerTMP;
    [SerializeField] Image hitFlash;
    [SerializeField] WeaponSlot[] weaponSlots;

    WeaponSlot currentWeapon;

    public void Init()
    {
        currentWeapon = weaponSlots[0];
        weaponSlots[0].Select(true, scaleMultiplier, 0, defaultWeaponSlotColor);
    }

    // �������� ������
    public void SelectWeapon(int index)
    {
        currentWeapon.Select(false, scaleMultiplier, animationTime, defaultWeaponSlotColor);

        currentWeapon = weaponSlots[index];

        currentWeapon.Select(true, scaleMultiplier, animationTime, defaultWeaponSlotColor);
    }

    // ��������� ����� � ����������� � ������� �������
    public void UpdateSideTurnText(bool show, bool isPlayerTurn = false)
    {
        string side = isPlayerTurn ? "Player's" : "Enemy's";

        turnTMP.text = show ? "turn" : "";
        turnSideTMP.text = show ? side : "";
        turnSideTMP.color = isPlayerTurn ? playerTurnColor : enemyTurnColor;
    }

    // ��������� ����� � ����������� � ���������� � ����
    public void UpdatePreparationText(float time, bool show = true)
    {
        timerTitleTMP.text = show ? "preparation" : "move ends in";
        timerTMP.text = time.ToString("F0");
    }

    // ��������� ����� �������
    public void UpdateTimer(float time)
    {
        timerTMP.text = time.ToString("F0");
    }

    // ��������/��������� ������ ������ ������
    public void WeaponSlotButtonsState(bool activate)
    {
        foreach (var slot in weaponSlots)
            slot.Button.interactable = activate;
    }

    // "�������" ��� ���������
    public void TriggerHitFlash()
    {
        hitFlash.DOFade(0.6f, hitFlashFadeTime).OnComplete(() =>
        {
            hitFlash.DOFade(0, hitFlashFadeTime);
        });
    }
}
