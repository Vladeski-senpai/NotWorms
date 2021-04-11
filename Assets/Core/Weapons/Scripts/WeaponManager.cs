using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] ShellManager shellPrefab;

    SoundManager soundManager;
    GameManager gameManager;
    WeaponMeta weaponMeta;
    Aim aim;

    void Start()
    {
        aim = GetComponent<Aim>();
        
        gameManager = GameManager.Instance;
        soundManager = SoundManager.Instance;

        ChangeWeapon(0);
    }

    // ������ ������ � ������������
    public void Shoot(float damage)
    {
        ShellManager shell = Instantiate(shellPrefab);
        Vector2 lookDirection = (aim.CursorPosition - transform.position).normalized;

        shell.transform.position = aim.SpawnPoint.position;
        shell.RBody.gravityScale = aim.ShellGravityScale;
        shell.RBody.velocity = lookDirection * aim.LaunchSpeed;

        shell.Init(true, damage);
        soundManager.Play(SoundName.Shoot, true);
    }

    // ��������
    public void Shoot()
    {
        ShellManager shell = Instantiate(shellPrefab);
        Vector2 lookDirection = (aim.CursorPosition - transform.position).normalized;

        shell.transform.position = aim.SpawnPoint.position;
        shell.RBody.gravityScale = aim.ShellGravityScale;
        shell.RBody.velocity = lookDirection * aim.LaunchSpeed;

        shell.Init(weaponMeta);
        soundManager.Play(SoundName.Shoot, true);
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
