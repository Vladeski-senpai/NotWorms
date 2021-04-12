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

    // Стреляем
    public void Shoot()
    {
        ShellManager shell = Instantiate(shellPrefab);

        shell.transform.position = aim.SpawnPoint.position;
        shell.RBody.gravityScale = aim.ShellGravityScale;
        shell.RBody.velocity = aim.LookDirection.normalized * aim.LaunchSpeed;

        shell.Init(weaponMeta);
        soundManager.Play(SoundName.Shoot, true);
    }

    // Меняем оружие
    public void ChangeWeapon(int index)
    {
        // Ищем мету оружиея по его типу
        foreach (var weapon in gameManager.WeaponsMeta.WeaponMetas)
            if (weapon.WeaponType == (WeaponType)index)
            {
                weaponMeta = weapon;

                break;
            }
    }
}
