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

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        characterController = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform; // ����� �������� ������
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
}

    //private InputAction moveAction;
    //private CharacterController characterController;
    //private float speedFactor = 5f; 
    //private float maxY = 20f; 
    //void Start()
    //{
    //    moveAction = InputSystem.actions.FindAction("Move");
    //    characterController = GetComponent<CharacterController>();
    //}

    //void Update()
    //{
    //    Vector2 moveValue = moveAction.ReadValue<Vector2>();
    //    Vector3 move = Camera.main.transform.forward;
    //    move.y = 0.0f;
    //    if(move == Vector3.zero)
    //    {
    //        move = Camera.main.transform.up;
    //    }
    //    move.Normalize();
    //    Vector3 moveForward = move;
    //    move += moveValue.x * Camera.main.transform.right;
    //    move.y = moveValue.y;
    //    move.y -= 30f * Time.deltaTime;
        
    //    characterController.Move(speedFactor * Time.deltaTime * move);
    //    this.transform.forward = moveForward;
        
    //}
//}