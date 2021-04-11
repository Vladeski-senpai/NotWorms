using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BotManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float startHealth;
    [SerializeField] float moveSpeed;
    [SerializeField] float damage;
    [SerializeField] float minShootDistance;
    [SerializeField] float maxShootDistance;
    [SerializeField] float minShootHeight;
    [SerializeField] float maxShootHeight;

    [Header("References")]
    [SerializeField] TextMeshProUGUI nicknameTMP;
    [SerializeField] Image healthSlider;
    [SerializeField] Transform aimPoint;
    [SerializeField] Rigidbody2D rbody;
    [SerializeField] ShootPoint shootPointPrefab;
    [SerializeField] ShellManager shellPrefab;

    public int TurnsSkip { get; set; }
    public bool MoveWasMade { get; set; }

    PlayerManager playerManager;
    GameManager gameManager;
    Transform shootPoint;
    Transform player;
    Helper helper;
    float health;
    bool canMove;

    void Start()
    {
        gameManager = GameManager.Instance;
        helper = Helper.Instance;
        playerManager = gameManager.PlayerManager;
        player = playerManager.transform;
        health = startHealth;

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
        transform.position = Vector2.MoveTowards(transform.position, shootPoint.position, moveSpeed * Time.deltaTime);
    }

    void Shoot()
    {
        helper.PerformWithDelay(1, () =>
        {
            var shell = Instantiate(shellPrefab);
            shell.Init(false, damage);
            shell.transform.position = aimPoint.position;

            float accuracy = Random.Range(2.9f, 3.6f); // 3.3f - 100% hit accuracy

            bool canShoot = BotAim.solve_ballistic_arc_lateral(aimPoint.position, player.position, accuracy,
                Random.Range(minShootHeight, maxShootHeight), out Vector3 s0, out float s1);

            if (!canShoot) return;

            shell.RBody.gravityScale = s1;
            shell.RBody.AddForce(s0 * accuracy, ForceMode2D.Impulse);
        });        
    }

    // Наносим урон боту
    public void DoDamage(float amount)
    {
        float finalHealth = health - amount;

        if (finalHealth <= 0)
        {
            finalHealth = 0;

            Die();
        }

        health = finalHealth;
        healthSlider.fillAmount = health / startHealth;
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
            if (shootPoint == null)
            {
                float shootDistance = Random.Range(minShootDistance, maxShootDistance);
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
        gameManager.OnMoveEnded();
        gameManager.DeleteBot(this);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("point"))
        {
            Destroy(shootPoint.gameObject);
            shootPoint = null;

            Shoot();
            canMove = false;
            gameManager.MovesSystem.StopTimer();
        }
    }
}
