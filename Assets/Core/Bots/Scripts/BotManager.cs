using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BotManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float startHealth;
    [SerializeField] float moveSpeed;

    [Header("References")]
    [SerializeField] TextMeshProUGUI nicknameTMP;
    [SerializeField] Image healthSlider;

    public int TurnsSkip { get; set; }
    public bool MoveWasMade { get; set; }

    PlayerManager playerManager;
    GameManager gameManager;
    Transform player;
    float health;
    bool canMove;

    void Start()
    {
        gameManager = GameManager.Instance;
        playerManager = gameManager.PlayerManager;
        player = playerManager.transform;
        health = startHealth;

        SetNickname();
        gameManager.RegisterBot(this);
    }

    void Update()
    {
        if (!canMove) return;

        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
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
        gameManager.DeleteBot(this);
        Destroy(gameObject);
    }
}
