using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Rigidbody2D rbody;
    [SerializeField] CircleCollider2D triggerCollider;
    [SerializeField] ParticleSystem deathPSPrefab;
    [SerializeField] ParticleSystem firePSPrefab;

    public Rigidbody2D RBody => rbody;

    PlayerManager playerManager;
    BotManager botManager;
    WeaponMeta weaponMeta;
    Coroutine damageCO;
    float impactDamage;
    int destroyTime = 50;
    bool isAlly; // true - ������ ������
    bool isDead; // true - ������ "����", �.�. ����� �������� ������ ������������� ����

    // �������������� ������ ������
    public void Init(WeaponMeta _weaponMeta)
    {
        weaponMeta = _weaponMeta;

        Init(true, weaponMeta.ImpactDamage);
    }

    // �������������� ������
    public void Init(bool _isAlly, float damage)
    {
        isAlly = _isAlly;
        impactDamage = damage;
       
        Destroy(gameObject, destroyTime);
    }

    // "�������" ������
    void Die()
    {
        isDead = true;
        triggerCollider.enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);

        var deathPS = Instantiate(deathPSPrefab);
        deathPS.transform.position = transform.position;
        Destroy(deathPS.gameObject, 60);

        SoundManager.Instance.Play(SoundName.Hit, true);
        CameraController.Instance.Shake();
    }

    // �������� �������� ������������� ����
    void StartPeriodicDamage()
    {
        damageCO = StartCoroutine(PeriodicDamage());

        var firePS = Instantiate(firePSPrefab, botManager.transform);
        firePS.transform.position = botManager.transform.position;

        Destroy(firePS.gameObject, weaponMeta.FireDuration);
        StartCoroutine(PeriodicDamageTimer());
    }

    // ������� ������������� ����
    IEnumerator PeriodicDamage()
    {
        while (botManager != null)
        {
            botManager.DoDamage(weaponMeta.PeriodicDamage, false);

            yield return new WaitForSeconds(weaponMeta.DamageInterval);
        }
    }

    // ������ �������������� �����
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
                botManager.DoDamage(impactDamage);
                //Die();

                // ���� ������ ������������ ����, �������� �������� ������������� ����
                if (weaponMeta.WeaponType == WeaponType.Fiery) StartPeriodicDamage();
                // ���� ������ ��������������� ����, ������������� ��� ����� �� n �������
                else if (weaponMeta.WeaponType == WeaponType.Frozen) botManager.FreezeState(weaponMeta.SkipTurnsCount);
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
