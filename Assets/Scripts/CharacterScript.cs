using Unity.Android.Gradle.Manifest;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterScript : MonoBehaviour
{
    private InputAction moveAction;
    private CharacterController characterController;

    private float baseSpeed = 7f;       // Базовая скорость
    private float groundSpeedFactor = 0.7f; // Замедление на земле
    private float airSpeedFactor = 1.2f;    // Ускорение в воздухе
    private float boostFactor = 1.5f;       // Множитель ускорения при Shift
    private float turnSpeed = 70f;       // Скорость поворота
    private float verticalSpeed = 5f;    // Скорость подъема и спуска
    private float gravity = 5f;          // Потеря высоты
    private float maxY = 70f;            // Максимальная высота
    private float currentVerticalSpeed = 0f; // Текущая вертикальная скорость

    private Transform cameraTransform;   // Ссылка на камеру

    private bool isOnGround = false;     // Персонаж на земле?

    private float burstPeriod = 10f;
    private float burstLeft;

    public float burstLevel => burstLeft / burstPeriod;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        characterController = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        GameState.AddListener(nameof(GameState.isBurst), OnBurstChanged);
    }

    void Update()
    {
        // Движение вперед в направлении камеры
        Vector3 moveDirection = cameraTransform.forward;
        moveDirection.y = 0; // Игнорируем наклон камеры, чтобы персонаж двигался горизонтально
        moveDirection.Normalize();

        // Повороты A и D
        float turn = Input.GetAxis("Horizontal"); // A/D или стрелки влево/вправо
        transform.Rotate(0, turn * turnSpeed * Time.deltaTime, 0);

        // Проверяем, находится ли персонаж на земле
        isOnGround = characterController.isGrounded;

        // Определяем скорость движения
        float speed = baseSpeed;
        if (isOnGround)
        {
            speed *= groundSpeedFactor; // Замедление на земле
        }
        else
        {
            speed *= airSpeedFactor; // Ускорение в воздухе
        }

        // Ускорение при нажатии Shift
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed *= boostFactor;
        }

        // Движение вперед
        moveDirection *= speed * Time.deltaTime;

        // Управление высотой
        if (Input.GetKey(KeyCode.W)) // Подъем
        {
            currentVerticalSpeed = verticalSpeed;
        }
        else if (Input.GetKey(KeyCode.S)) // Снижение
        {
            currentVerticalSpeed = -verticalSpeed;
        }
        else // Гравитация
        {
            currentVerticalSpeed -= gravity * Time.deltaTime;
        }

        // Ограничение высоты
        float nextY = transform.position.y + currentVerticalSpeed * Time.deltaTime;
        if (nextY > maxY)
        {
            currentVerticalSpeed = 0f; // Останавливаем подъем
            nextY = maxY;
        }
        else if (nextY < 0f)
        {
            currentVerticalSpeed = 0f;
            nextY = 0f;
        }

        // Применяем вертикальную скорость
        moveDirection.y = currentVerticalSpeed * Time.deltaTime;

        // Перемещаем персонажа
        characterController.Move(moveDirection);

        // Наклон персонажа
        this.transform.forward = cameraTransform.forward;
    }

    private void LateUpdate()
    {
        if(burstLeft > 0f)
        {
            burstLeft -= Time.deltaTime;
            if(burstLeft < 0f)
            {
                burstLeft = 0f;
                GameState.isBurst = false;
            }
        }
    }

    private void OnBurstChanged(string ignored)
    {
        if (GameState.isBurst)
        {
            Debug.Log("Burst");
        }
    }

    private void OnDestroy()
    {
        GameState.RemoveListener(nameof(GameState.isBurst), OnBurstChanged);
    }

}

   
