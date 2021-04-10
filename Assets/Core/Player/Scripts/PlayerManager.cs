using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float startHealth;
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpStrength;
    [SerializeField] float jumpTimer;
    [SerializeField] float jumpHoldTimer;
    [SerializeField] float checkRadius;
    [SerializeField] LayerMask groundLM;

    [Header("References")]
    [SerializeField] TextMeshProUGUI nicknameTMP;
    [SerializeField] Rigidbody2D rbody;
    [SerializeField] GameObject crosshair;
    [SerializeField] Transform groundCheck;
    [SerializeField] Image healthSlider;

    [Header("Other")]
    [SerializeField] Color groundCheckColor;
    [SerializeField] bool drawGizmos;

    [Header("Events")]
    [SerializeField] UnityEvent<int> OnSelectWeaponEvent;

    WeaponManager weaponManager;
    GameManager gameManager;
    Director director;
    Aim aim;
    Vector2 moveDirection;
    float health;
    float jumpTime;
    float jumpHoldTime;
    bool isJumpHolding;
    bool onGround;
    bool canShoot;
    bool canJump;
    bool canMove;

    void Start()
    {
        weaponManager = GetComponent<WeaponManager>();
        aim = GetComponent<Aim>();

        gameManager = GameManager.Instance;
        director = Director.Instance;
        nicknameTMP.text = director.GameMeta.PlayerName;
        nicknameTMP.color = director.GameMeta.PlayerNameColor;
        health = startHealth;
    }

    void Update()
    {
        // TEMP
        if (Input.GetKeyDown(KeyCode.H)) DoDamage(25);

        if (!canMove) return;

        WeaponChooserHandler();
        JumpHandler();
        AimHandler();
        MovementHandler();
        ShootingHandler();
    }

    void FixedUpdate()
    {
        if (!canMove) return;

        rbody.position += moveDirection.normalized * moveSpeed * Time.fixedDeltaTime;
    }

    #region Handlers

    // Передвижение
    void MovementHandler()
    {
        moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
    }

    // Обработчик прыжков
    void JumpHandler()
    {
        jumpTime -= Time.deltaTime;

        // Если истекло время для второго прыжка
        if (canJump && jumpTime < 0) canJump = false;

        // Прыжок
        if (Input.GetKeyDown(KeyCode.Space))
        {
            onGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLM);
            jumpTime = jumpTimer;
            jumpHoldTime = jumpHoldTimer;
            isJumpHolding = true;
            canShoot = false;
            Cursor.visible = true;
            crosshair.SetActive(false);
            aim.PointsState(false);

            if (onGround)
            {
                if (canJump)
                {
                    canJump = false;
                    DoJump();
                }
                else canJump = true;
            }
        }
    }

    // Обработчик прицеливания
    void AimHandler()
    {
        jumpHoldTime -= Time.deltaTime;

        if (isJumpHolding && jumpHoldTime < 0 && !canShoot)
        {
            canShoot = true;
            crosshair.SetActive(true);
            aim.PointsState(true);
            Cursor.visible = false;
        }

        if (Input.GetKeyUp(KeyCode.Space)) isJumpHolding = false;
    }

    // Обработчик стрельбы
    void ShootingHandler()
    {
        if (canShoot)
        {
            aim.UpdateTrajectory();

            if (Input.GetMouseButtonDown(0))
            {
                weaponManager.Shoot();
                gameManager.OnMoveEnded();
            }
        }
    }

    // Обработчик выбора оружия !!!!!!!!!!!! TEMPORARY !!!!!!!!!!!!!
    void WeaponChooserHandler()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            OnSelectWeaponEvent.Invoke(0);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            OnSelectWeaponEvent.Invoke(1);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            OnSelectWeaponEvent.Invoke(2);
    }

    #endregion

    // Наносим урон игроку
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

    // Воскрешаем игрока
    public void RespawnPlayer()
    {
        health = startHealth * 0.5f;
        healthSlider.fillAmount = health / startHealth;

        gameObject.SetActive(true);
    }

    // Может ли игрок ходить
    public void TurnState(bool _canMove)
    {
        canMove = _canMove;
        moveDirection = Vector2.zero;
        canShoot = false;
        isJumpHolding = false;
        Cursor.visible = canMove;

        aim.PointsState(false);
        crosshair.SetActive(false);
    }

    // Прыгаем
    void DoJump()
    {
        rbody.AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);
    }

    // Убиваем игрока
    void Die()
    {
        moveDirection = Vector2.zero;

        gameManager.GameOver();
        gameObject.SetActive(false);
    }

    void OnDrawGizmos()
    {
        if (!drawGizmos) return;

        Gizmos.color = groundCheckColor;
        Gizmos.DrawSphere(groundCheck.position, checkRadius);
    }
}
