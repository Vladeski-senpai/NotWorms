using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] ShellManager shellPrefab;

    GameManager gameManager;
    WeaponMeta weaponMeta;
    Aim aim;

    void Start()
    {
        aim = GetComponent<Aim>();
        gameManager = GameManager.Instance;

        ChangeWeapon(0);
    }

    // ������ ������ � ������������
    public void Shoot(float damage)
    {
        ShellManager shell = Instantiate(shellPrefab);
        Vector2 lookDirection = (aim.CursorPosition - transform.position).normalized;

        shell.transform.position = aim.SpawnPoint.position;
        shell.RBody.drag = aim.ShellLinearDrag;
        shell.RBody.gravityScale = aim.ShellGravityScale;
        shell.RBody.velocity = lookDirection * aim.LaunchSpeed;

        shell.Init(true, damage);
    }

    // ��������
    public void Shoot()
    {
        ShellManager shell = Instantiate(shellPrefab);
        Vector2 lookDirection = (aim.CursorPosition - transform.position).normalized;

        shell.transform.position = aim.SpawnPoint.position;
        shell.RBody.drag = aim.ShellLinearDrag;
        shell.RBody.gravityScale = aim.ShellGravityScale;
        shell.RBody.velocity = lookDirection * aim.LaunchSpeed;

        shell.Init(weaponMeta);
    }

    // ������ ������
    public void ChangeWeapon(int index)
    {
        // ���� ���� ������� �� ��� ����
        foreach (var weapon in gameManager.WeaponsMeta.WeaponMetas)
            if (weapon.WeaponType == (WeaponType)index)
            {
                weaponMeta = weapon;

                break;
            }
    }
}
