using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Rigidbody2D rbody;
    [SerializeField] CircleCollider2D triggerCollider;

    public Rigidbody2D RBody => rbody;

    PlayerManager playerManager;
    BotManager botManager;
    WeaponMeta weaponMeta;
    Coroutine damageCO;
    float impactDamage;
    int destroyTime = 50;
    bool isAlly; // true - снаряд игрока
    bool isDead; // true - снаряд "мёртв", т.е. может наносить только переодический урон

    // Инициализируем снаряд игрока
    public void Init(WeaponMeta _weaponMeta)
    {
        weaponMeta = _weaponMeta;

        Init(true, weaponMeta.ImpactDamage);
    }

    // Инициализируем снаряд
    public void Init(bool _isAlly, float damage)
    {
        isAlly = _isAlly;
        impactDamage = damage;
       
        Destroy(gameObject, destroyTime);
    }

    // "Убиваем" снаряд
    void Die()
    {
        isDead = true;
        triggerCollider.enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    // Начинаем наносить переодический урон
    void StartPeriodicDamage()
    {
        damageCO = StartCoroutine(PeriodicDamage());

        StartCoroutine(PeriodicDamageTimer());
    }

    // Наносим переодический урон
    IEnumerator PeriodicDamage()
    {
        while (botManager != null)
        {
            botManager.DoDamage(weaponMeta.PeriodicDamage);

            yield return new WaitForSeconds(weaponMeta.DamageInterval);
        }
    }

    // Таймер переодического урона
    IEnumerator PeriodicDamageTimer()
    {
        yield return new WaitForSeconds(weaponMeta.FireDuration);

        if (damageCO != null)
            StopCoroutine(damageCO);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        if (isAlly)
        {
            botManager = collision.transform.GetComponent<BotManager>();

            if (botManager != null)
            {
                botManager.DoDamage(impactDamage, true);
                //Die();

                // Если оружие поджигающего типа, начинаем наносить переодический урон
                if (weaponMeta.WeaponType == WeaponType.Fiery) StartPeriodicDamage();
                // Если оружие замораживающего типа, останавливаем ход врагу на n раундов
                else if (weaponMeta.WeaponType == WeaponType.Frozen) botManager.TurnsSkip += weaponMeta.SkipTurnsCount;
            }
        }
        else
        {
            playerManager = collision.transform.GetComponent<PlayerManager>();

            if (playerManager != null)
            {
                playerManager.DoDamage(impactDamage);
                //Die();
            }
        }

        Die();
        GameManager.Instance.OnMoveEnded();
    }
}
