using Unity.Android.Gradle.Manifest;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterScript : MonoBehaviour
{
    private InputAction moveAction;
    private CharacterController characterController;

    private float baseSpeed = 7f;       // ������� ��������
    private float groundSpeedFactor = 0.7f; // ���������� �� �����
    private float airSpeedFactor = 1.2f;    // ��������� � �������
    private float boostFactor = 1.5f;       // ��������� ��������� ��� Shift
    private float turnSpeed = 70f;       // �������� ��������
    private float verticalSpeed = 5f;    // �������� ������� � ������
    private float gravity = 5f;          // ������ ������
    private float maxY = 70f;            // ������������ ������
    private float currentVerticalSpeed = 0f; // ������� ������������ ��������

    private Transform cameraTransform;   // ������ �� ������

    private bool isOnGround = false;     // �������� �� �����?

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
        // �������� ������ � ����������� ������
        Vector3 moveDirection = cameraTransform.forward;
        moveDirection.y = 0; // ���������� ������ ������, ����� �������� �������� �������������
        moveDirection.Normalize();

        // �������� A � D
        float turn = Input.GetAxis("Horizontal"); // A/D ��� ������� �����/������
        transform.Rotate(0, turn * turnSpeed * Time.deltaTime, 0);

        // ���������, ��������� �� �������� �� �����
        isOnGround = characterController.isGrounded;

        // ���������� �������� ��������
        float speed = baseSpeed;
        if (isOnGround)
        {
            speed *= groundSpeedFactor; // ���������� �� �����
        }
        else
        {
            speed *= airSpeedFactor; // ��������� � �������
        }

        // ��������� ��� ������� Shift
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed *= boostFactor;
        }

        // �������� ������
        moveDirection *= speed * Time.deltaTime;

        // ���������� �������
        if (Input.GetKey(KeyCode.W)) // ������
        {
            currentVerticalSpeed = verticalSpeed;
        }
        else if (Input.GetKey(KeyCode.S)) // ��������
        {
            currentVerticalSpeed = -verticalSpeed;
        }
        else // ����������
        {
            currentVerticalSpeed -= gravity * Time.deltaTime;
        }

        // ����������� ������
        float nextY = transform.position.y + currentVerticalSpeed * Time.deltaTime;
        if (nextY > maxY)
        {
            currentVerticalSpeed = 0f; // ������������� ������
            nextY = maxY;
        }
        else if (nextY < 0f)
        {
            currentVerticalSpeed = 0f;
            nextY = 0f;
        }

        // ��������� ������������ ��������
        moveDirection.y = currentVerticalSpeed * Time.deltaTime;

        // ���������� ���������
        characterController.Move(moveDirection);

        // ������ ���������
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

   
