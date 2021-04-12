using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BotManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI nicknameTMP;
    [SerializeField] Image healthSlider;
    [SerializeField] Transform aimPoint;
    [SerializeField] Rigidbody2D rbody;
    [SerializeField] ShootPoint shootPointPrefab;
    [SerializeField] ShellManager shellPrefab;
    [SerializeField] GameObject frozenEffect;
    [SerializeField] ParticleSystem deathPSPrefab;
    [SerializeField] ParticleSystem hitPSPrefab;

    public int TurnsSkip { get; private set; }
    public bool MoveWasMade { get; set; }

    PlayerManager playerManager;
    BotsSettings botsSettings;
    GameManager gameManager;
    Transform shootPoint;
    Transform player;
    float health;
    bool hasShooted;
    bool canMove;

    void Start()
    {
        gameManager = GameManager.Instance;
        botsSettings = Director.Instance.BotsSettings;
        playerManager = gameManager.PlayerManager;
        player = playerManager.transform;
        health = botsSettings.StartHealth;

        SetNickname();
        gameManager.RegisterBot(this);

        Vector2 newPos = transform.position;
        newPos.x = Random.Range(-48, 48);
        transform.position = newPos;
    }

    void Update()
    {
        if (!canMove) return;

        Movement();
    }

    void Movement()
    {
        transform.position = Vector2.MoveTowards(transform.position, shootPoint.position, botsSettings.MoveSpeed * Time.deltaTime);
    }

    void Shoot()
    {
        // accuracy = 3.3f - 100% hit accuracy
        float accuracy = Random.Range(botsSettings.MinAccuracy, botsSettings.MaxAccuracy);

        bool canShoot = BotAim.solve_ballistic_arc_lateral(aimPoint.position, player.position, accuracy,
            Random.Range(botsSettings.MinShootHeight, botsSettings.MaxShootHeight), out Vector3 s0, out float s1);

        if (!canShoot) return;

        var shell = Instantiate(shellPrefab);
        shell.Init(false, botsSettings.Damage);
        shell.transform.position = aimPoint.position;
        shell.RBody.gravityScale = s1;
        shell.RBody.AddForce(s0 * accuracy, ForceMode2D.Impulse);

        SoundManager.Instance.Play(SoundName.Shoot, true);
    }

    // Наносим урон боту
    public void DoDamage(float amount, bool hitFlash = true)
    {
        float finalHealth = health - amount;

        if (finalHealth <= 0)
        {
            finalHealth = 0;

            Die();
        }
        else if (hitFlash)
        {
            var hitPs = Instantiate(hitPSPrefab);
            Vector2 newPos = transform.position;
            newPos.y += 1.2f;
            hitPs.transform.position = newPos;
            Destroy(hitPs.gameObject, 20);
        }

        health = finalHealth;
        healthSlider.fillAmount = health / botsSettings.StartHealth;

        if (hitFlash) gameManager.HUDManager.TriggerHitFlash();
    }

    // Может ли игрок ходить
    public void TurnState(bool _canMove)
    {
        canMove = _canMove;

        if (canMove)
        {
            if (shootPoint == null || Vector2.Distance(transform.position, player.position) > botsSettings.ShootPointDestroyDistance)
            {
                hasShooted = false;

                if (shootPoint != null)
                    Destroy(shootPoint.gameObject);

                float shootDistance = Random.Range(botsSettings.MinShootDistance, botsSettings.MaxShootDistance);
                var point = Instantiate(shootPointPrefab);

                point.SetPosition(player, transform.position.y, shootDistance);
                shootPoint = point.transform;
            }
        }
    }

    // "Замораживаем" на несколько ходов
    public void FreezeState(int moves)
    {
        TurnsSkip += moves;
        frozenEffect.SetActive(TurnsSkip > 0);
    }

    // Устанавливаем случайный ник
    void SetNickname()
    {
        string[] names = Director.Instance.GameSettings.BotNames;

        nicknameTMP.text = "Bot " + names[Random.Range(0, names.Length)];
    }

    // Убиваем бота
    void Die()
    {
        var deathPS = Instantiate(deathPSPrefab);
        deathPS.transform.position = transform.position;
        Destroy(deathPS.gameObject, 60);

        gameManager.OnMoveEnded();
        gameManager.DeleteBot(this);
        Destroy(gameObject);
    }

    IEnumerator PrepareShooting()
    {
        yield return new WaitForSeconds(botsSettings.ShootDelay);

        Shoot();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasShooted || TurnsSkip > 0) return;

        if (collision.CompareTag("point"))
        {
            if (collision.transform == shootPoint)
            {
                if (shootPoint != null) Destroy(shootPoint.gameObject);

                shootPoint = null;
                hasShooted = true;
                canMove = false;
                gameManager.MovesSystem.StopTimer();
                StartCoroutine(PrepareShooting());
            }
        }
    }
}
