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
    bool isAlly; // true - снар€д игрока
    bool isDead; // true - снар€д "мЄртв", т.е. может наносить только переодический урон

    // »нициализируем снар€д игрока
    public void Init(WeaponMeta _weaponMeta)
    {
        weaponMeta = _weaponMeta;

        Init(true, weaponMeta.ImpactDamage);
    }

    // »нициализируем снар€д
    public void Init(bool _isAlly, float damage)
    {
        isAlly = _isAlly;
        impactDamage = damage;

        // 9 - Player Shell layer, 10 - Enemy Shell layer
        LayerMask.LayerToName(isAlly ? 9 : 10);
        Destroy(gameObject, destroyTime);
    }

    // "”биваем" снар€д
    void Die()
    {
        isDead = true;
        triggerCollider.enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    // Ќачинаем наносить переодический урон
    void StartPeriodicDamage()
    {
        damageCO = StartCoroutine(PeriodicDamage());

        Helper.Instance.PerformWithDelay(weaponMeta.FireDuration, () =>
        {
            if (damageCO != null)
                StopCoroutine(damageCO);
        });
    }

    // Ќаносим переодический урон
    IEnumerator PeriodicDamage()
    {
        while (botManager != null)
        {
            botManager.DoDamage(weaponMeta.PeriodicDamage);

            yield return new WaitForSeconds(weaponMeta.DamageInterval);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        if (isAlly)
        {
            botManager = collision.transform.GetComponent<BotManager>();

            if (botManager != null)
            {
                botManager.DoDamage(impactDamage);
                Die();

                // ≈сли оружие поджигающего типа, начинаем наносить переодический урон
                if (weaponMeta.WeaponType == WeaponType.Fiery) StartPeriodicDamage();
                // ≈сли оружие замораживающего типа, останавливаем ход врагу на n раундов
                else if (weaponMeta.WeaponType == WeaponType.Frozen) botManager.TurnsSkip += weaponMeta.SkipTurnsCount;
            }
        }
        else
        {
            playerManager = collision.transform.GetComponent<PlayerManager>();

            if (playerManager != null)
            {
                playerManager.DoDamage(impactDamage);
                Die();
            }
        }
    }
}
