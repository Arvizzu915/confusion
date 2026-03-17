using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLook : MonoBehaviour
{
    [SerializeField] private float sensibilityX = 3, sensibilityY = 3;
    [SerializeField] private Transform orientation;

    private float xRotation, yRotation;

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
        float mouseX = Look.ReadValue<Vector2>().x * Time.deltaTime * sensibilityX;
        float mouseY = Look.ReadValue<Vector2>().y * Time.deltaTime * sensibilityY;

        yRotation += mouseX;

        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation =  Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
