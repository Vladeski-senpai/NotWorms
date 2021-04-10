using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float animationTime;
    [SerializeField] float scaleMultiplier;
    [SerializeField] Color defaultColor;

    [Header("References")]
    [SerializeField] WeaponSlot[] weaponSlots;

    WeaponSlot currentWeapon;

    void Start()
    {
        currentWeapon = weaponSlots[0];
        weaponSlots[0].Select(true, scaleMultiplier, 0, defaultColor);
    }

    // Выбираем оружие
    public void SelectWeapon(int index)
    {
        currentWeapon.Select(false, scaleMultiplier, animationTime, defaultColor);

        currentWeapon = weaponSlots[index];

        currentWeapon.Select(true, scaleMultiplier, animationTime, defaultColor);
    }
}
