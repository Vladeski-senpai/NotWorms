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
    [Tooltip("��� ������")]
    [SerializeField] WeaponType weaponType;
    [Tooltip("��������� �������")]
    [SerializeField] float shellDistance;
    [Tooltip("���� �� ���������")]
    [SerializeField] float impactDamage;
    [Tooltip("�������� �������")]
    [SerializeField] float launchSpeed;
    [Tooltip("������ �������")]
    [SerializeField] float shellLinearDrag;
    [Tooltip("����� ���������� �������")]
    [SerializeField] float shellGravityScale;

    [Header("Weapon type - Fiery settings")]
    [Tooltip("������������� ���� (���� �� ���)")]
    [SerializeField] float periodicDamage;
    [Tooltip("�������� ����� ���������� �����")]
    [SerializeField] float damageInterval;
    [Tooltip("������������ \"�������\"")]
    [SerializeField] float fireDuration;

    [Header("Weapon type - Frozen settings")]
    [Tooltip("���-�� ������������ �����")]
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