using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float checkRadius;
    [SerializeField] LayerMask groundLM;

    [Header("References")]
    [SerializeField] TextMeshProUGUI nicknameTMP;
    [SerializeField] Rigidbody2D rbody;
    [SerializeField] GameObject crosshair;
    [SerializeField] Transform groundCheck;
    [SerializeField] Image healthSlider;
    [SerializeField] RectTransform canvasRect;
    [SerializeField] GraphicRaycaster m_Raycaster;
    [SerializeField] EventSystem m_EventSystem;

    [Header("Other")]
    [SerializeField] Color groundCheckColor;
    [SerializeField] bool drawGizmos;

    [Header("Events")]
    [SerializeField] UnityEvent<int> OnSelectWeaponEvent;

    public Vector2 TouchPosition { get; private set; }

    List<RaycastResult> raycastResults;
    PointerEventData m_PointerEventData;
    CameraController cameraController;
    WeaponManager weaponManager;
    PlayerSettings playerSettings;
    InputHandler inputHandler;
    GameManager gameManager;
    Director director;
    Aim aim;

    Vector2 moveDirection;
    Vector2 firstTouchPos;

    float health;
    float jumpTime;
    float aimHoldTime;
    bool isTouchingPlayer;
    bool isJumpHolding;
    bool defaultCursor;
    bool onGround;
    bool canShoot;
    bool canJump;
    bool canMove;
    bool isAndroid;

    void Start()
    {
        weaponManager = GetComponent<WeaponManager>();
        inputHandler = GetComponent<InputHandler>();
        aim = GetComponent<Aim>();

        gameManager = GameManager.Instance;
        director = Director.Instance;
        playerSettings = director.PlayerSettings;
        cameraController = gameManager.CameraController;
        nicknameTMP.text = director.GameMeta.PlayerName;
        nicknameTMP.color = director.GameMeta.PlayerNameColor;
        defaultCursor = director.GameMeta.DefaultCursor;
        health = playerSettings.StartHealth;

#if UNITY_ANDROID
        isAndroid = true;
#endif
    }

    void Update()
    {
        if (!canMove) return;

        if (!isAndroid)
        {
            WeaponChooserHandler();
            JumpHandler();
        }
        else TouchesHandler();

        AimHandler();
        MovementHandler();
        ShootingHandler();
    }

    void FixedUpdate()
    {
        if (!canMove || isTouchingPlayer || (isJumpHolding && jumpTime > 0)) return;

        rbody.position += moveDirection.normalized * playerSettings.MoveSpeed * Time.fixedDeltaTime;
    }

#region Handlers

    // Обработчик касаний
    void TouchesHandler()
    {
        JumpHandler();

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    firstTouchPos = touch.position;
                    TouchPosition = touch.position;
                    isTouchingPlayer = inputHandler.CheckForPlayer(firstTouchPos);

                    if (CheckUIHit()) return;

                    CheckJump();
                    aim.CheckMoveDirection();

                    //nicknameTMP.text = inputHandler.CheckForPlayer().ToString();
                    break;

                case TouchPhase.Stationary:
                    TouchPosition = touch.position;

                    if (CheckUIHit()) return;

                    aim.CheckMoveDirection();
                    break;

                case TouchPhase.Ended:
                    TouchPosition = touch.position;
                    isTouchingPlayer = false;

                    TouchPosition = Vector2.zero;
                    aim.MoveDirection = 0;

                    if (canShoot)
                    {
                        weaponManager.Shoot();
                        gameManager.MovesSystem.StopTimer();
                        TurnState(false);
                    }

                    isJumpHolding = false;
                    break;
            }
        }
    }

    // Передвижение
    void MovementHandler()
    {
        moveDirection = new Vector2(isAndroid ? aim.MoveDirection : Input.GetAxisRaw("Horizontal"), 0);
    }

    // Обработчик прыжков
    void JumpHandler()
    {
        jumpTime -= Time.deltaTime;

        // Если истекло время для второго прыжка
        if (canJump && jumpTime < 0) canJump = false;

        if (!isAndroid && Input.GetKeyDown(KeyCode.Space)) CheckJump();  // Прыжок
    }

    // Обработчик прицеливания
    void AimHandler()
    {
        aimHoldTime -= Time.deltaTime;

        if (isJumpHolding && isTouchingPlayer && aimHoldTime < 0 && !canShoot)
        {
            canShoot = true;
            crosshair.SetActive(defaultCursor);
            aim.PointsState(true);
        }

        if (!isAndroid && Input.GetKeyUp(KeyCode.Space)) isJumpHolding = false;
    }

    // Обработчик стрельбы
    void ShootingHandler()
    {
        if (canShoot)
        {
            aim.UpdateTrajectory();

            if (!isAndroid && Input.GetMouseButtonDown(0))
            {
                weaponManager.Shoot();
                gameManager.MovesSystem.StopTimer();
                TurnState(false);
            }
        }
    }

    // Обработчик выбора оружия
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

    #region Public Method's
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
        healthSlider.fillAmount = health / playerSettings.StartHealth;
        cameraController.Shake(0);
    }

    // Воскрешаем игрока
    public void RespawnPlayer()
    {
        health = playerSettings.StartHealth * 0.5f;
        healthSlider.fillAmount = health / playerSettings.StartHealth;

        gameObject.SetActive(true);
    }

    // Может ли игрок ходить
    public void TurnState(bool _canMove)
    {
        canMove = _canMove;
        moveDirection = Vector2.zero;
        TouchPosition = Vector2.zero;
        canShoot = false;
        isJumpHolding = false;
        isTouchingPlayer = false;
        aim.MoveDirection = 0;

        aim.PointsState(false);
        crosshair.SetActive(false);
    }

    #endregion

    #region Private Method's

    // Проверяем можем ли прыгнуть
    void CheckJump()
    {
        onGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLM);
        jumpTime = playerSettings.JumpTimer;
        aimHoldTime = playerSettings.AimHoldTimer;
        isJumpHolding = true;
        canShoot = false;
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

    // Проверяем нажали ли на UI элементы
    bool CheckUIHit()
    {
        //Set up the new Pointer Event
        m_PointerEventData = new PointerEventData(m_EventSystem);
        //Set the Pointer Event Position to that of the game object
        m_PointerEventData.position = TouchPosition;

        //Create a list of Raycast Results
        raycastResults = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        m_Raycaster.Raycast(m_PointerEventData, raycastResults);

        if (raycastResults.Count > 0)
        {
            //raycastResults[0].gameObject.name;
            return true;
        }
        else
        {
            return false;
        }
    }

    // Прыгаем
    void DoJump()
    {
        rbody.AddForce(Vector2.up * playerSettings.JumpStrength, ForceMode2D.Impulse);
    }

    // Убиваем игрока
    void Die()
    {
        moveDirection = Vector2.zero;

        gameManager.GameOver();
        gameObject.SetActive(false);
    }

    /*void OnDrawGizmos()
    {
        if (!drawGizmos) return;

        Gizmos.color = groundCheckColor;
        Gizmos.DrawSphere(groundCheck.position, checkRadius);
    }*/

    #endregion
}
