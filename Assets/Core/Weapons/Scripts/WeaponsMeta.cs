using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponsMeta", menuName = "Meta/WeaponsMeta", order = 1)]
public class WeaponsMeta : ScriptableObject
{
    [SerializeField] List<WeaponMeta> weaponMetas;

    public List<WeaponMeta> WeaponMetas => weaponMetas;
}

[System.Serializable]
public class WeaponMeta
{
    [Header("Common settings")]
    [Tooltip("Тип оружия")]
    [SerializeField] WeaponType weaponType;
    [Tooltip("Дальность снаряда")]
    [SerializeField] float shellDistance;
    [Tooltip("Урон от попадания")]
    [SerializeField] float impactDamage;
    [Tooltip("Скорость снаряда")]
    [SerializeField] float launchSpeed;
    [Tooltip("Трение снаряда")]
    [SerializeField] float shellLinearDrag;
    [Tooltip("Скейл гравитации снаряда")]
    [SerializeField] float shellGravityScale;

    [Header("Weapon type - Fiery settings")]
    [Tooltip("Переодический урон (урон за тик)")]
    [SerializeField] float periodicDamage;
    [Tooltip("Интервал между нанесением урона")]
    [SerializeField] float damageInterval;
    [Tooltip("Длительность \"Поджога\"")]
    [SerializeField] float fireDuration;

    [Header("Weapon type - Frozen settings")]
    [Tooltip("Кол-во пропускаемых ходов")]
    [SerializeField] int skipTurnsCount;


    // Common settings
    public WeaponType WeaponType { get => weaponType; set => weaponType = value; }
    public float ShellDistance { get => shellDistance; set => shellDistance = value; }
    public float ImpactDamage { get => impactDamage; set => impactDamage = value; }
    public float LaunchSpeed { get => launchSpeed; set => launchSpeed = value; }
    public float ShellLinearDrag { get => shellLinearDrag; set => shellLinearDrag = value; }
    public float ShellGravityScale { get => shellGravityScale; set => shellGravityScale = value; }

    // Weapon type - Fierly settings
    public float PeriodicDamage { get => periodicDamage; set => periodicDamage = value; }
    public float DamageInterval { get => damageInterval; set => damageInterval = value; }
    public float FireDuration { get => fireDuration; set => fireDuration = value; }

    // Weapon type - Frozen settings
    public int SkipTurnsCount { get => skipTurnsCount; set => skipTurnsCount = value; }
}