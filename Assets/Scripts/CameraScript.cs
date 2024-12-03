using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    private GameObject character;
    private Vector3 s;
    private InputAction lookAction;
    private float angleH, angleH0;
    private float angleV, angleV0;

    void Start()
    {
        character = GameObject.Find("Character");
        lookAction = InputSystem.actions.FindAction("Look");
        s = this.transform.position - character.transform.position;
        angleH0 = angleH = transform.eulerAngles.y;
        angleV0 = angleV = transform.eulerAngles.x;
    }

    void Update()
    {
        Vector2 lookValue = lookAction.ReadValue<Vector2>();
        angleH += lookValue.x * 0.05f;
        angleV -= lookValue.y * 0.05f;
        if(angleV <= 0f) 
        { 
            angleV = 0f;
        }
        else if(angleV >= 90f)
        {
            angleV = 90f;
        }
        this.transform.eulerAngles = new Vector3(angleV, angleH, 0f);
        this.transform.position = character.transform.position + 
            Quaternion.Euler(angleV - angleV0, angleH - angleH0, 0f) * s;
    }
}
