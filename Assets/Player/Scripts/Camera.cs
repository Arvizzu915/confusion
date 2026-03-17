using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class CameraLook : MonoBehaviour
{
    [SerializeField] private float sensibilityX = 3, sensibilityY = 3;
    [SerializeField] private Transform orientation;

    private float xRotation, yRotation;

    [SerializeField] private float crouchingHeight = 0, standingHeight = 0.5f;
    private float targetHeigth = 0.5f;

    [SerializeField] private PlayerMovement playerMovement;

    private InputAction Look;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Look = playerMovement.inputManager.inputs.Playing.Look;
    }

    private void Update()
    {
        Mathf.Lerp(transform.position.y, targetHeigth, Time.deltaTime * 10f);

        float mouseX = Look.ReadValue<Vector2>().x * Time.deltaTime * sensibilityX;
        float mouseY = Look.ReadValue<Vector2>().y * Time.deltaTime * sensibilityY;

        yRotation += mouseX;

        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation =  Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public void ChangePosition(bool crouching)
    {
        if (crouching)
        {
            targetHeigth = crouchingHeight;
        }
        else
        {
            targetHeigth = standingHeight;
        }
    }
}
