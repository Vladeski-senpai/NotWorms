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
    [SerializeField] ParticleSystem deathPSPrefab;

    public int TurnsSkip { get; set; }
    public bool MoveWasMade { get; set; }

    PlayerManager playerManager;
    BotsSettings botsSettings;
    GameManager gameManager;
    Transform shootPoint;
    Transform player;
    float health;
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
    public void DoDamage(float amount, bool shake = false)
    {
        float finalHealth = health - amount;

        if (finalHealth <= 0)
        {
            finalHealth = 0;

            Die();
        }

        health = finalHealth;
        healthSlider.fillAmount = health / botsSettings.StartHealth;
    }

    // Может ли игрок ходить
    public bool TurnState(bool _canMove)
    {
        if (TurnsSkip > 0)
        {
            TurnsSkip--;
            return false;
        }
        else canMove = _canMove;

        if (canMove)
        {
            if (shootPoint == null || Vector2.Distance(transform.position, player.position) > botsSettings.ShootPointDestroyDistance)
            {
                if (shootPoint != null)
                    Destroy(shootPoint.gameObject);

                float shootDistance = Random.Range(botsSettings.MinShootDistance, botsSettings.MaxShootDistance);
                var point = Instantiate(shootPointPrefab);

                point.SetPosition(player, transform.position.y, shootDistance);
                shootPoint = point.transform;
            }
        }

        return true;
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
        if (collision.CompareTag("point"))
        {
            if (shootPoint != null) Destroy(shootPoint.gameObject);

            shootPoint = null;

            canMove = false;
            gameManager.MovesSystem.StopTimer();
            StartCoroutine(PrepareShooting());
        }
    }
}
