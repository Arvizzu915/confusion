using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.Rendering;

public class CameraLook : MonoBehaviour
{
    [SerializeField] private float sensibilityX = 3, sensibilityY = 3;
    [SerializeField] private Transform orientation;

    private float xRotation, yRotation;

    [SerializeField] private float crouchingHeight = 0, standingHeight = 0.5f;
    private float targetHeight = 1.5f;

    [SerializeField] private PlayerMovement playerMovement;

    private InputAction Look;

    private int sens = 0;

    private void Start()
    {
        ChangePosition(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Look = playerMovement.inputManager.inputs.Playing.Look;

        PlayerManager.instance.inputManager.inputs.Playing.ChangeSens.performed += ChangeSensibility;
    }

    private void OnDisable()
    {
        PlayerManager.instance.inputManager.inputs.Playing.ChangeSens.performed -= ChangeSensibility;
    }

    private void LateUpdate()
    {
        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            new Vector3(transform.localPosition.x, targetHeight, transform.localPosition.z),
            Time.deltaTime * 10f
        );

        Vector2 lookInput = Look.ReadValue<Vector2>();

        float mouseX = lookInput.x * sensibilityX;
        float mouseY = lookInput.y * sensibilityY;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        orientation.rotation = Quaternion.Euler(0f, yRotation, 0f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    public void ChangePosition(bool crouching)
    {
        if (crouching)
        {
            targetHeight = crouchingHeight;
        }
        else
        {
            targetHeight = standingHeight;
        }
    }

    private void ChangeSensibility(InputAction.CallbackContext ctx)
    {
        if (sens == 0)
        {
            sensibilityX = 70;
            sensibilityY = 70;

            sens = 1;
        }
        else if (sens == 1)
        {
            sensibilityX = 0.5f;
            sensibilityY = 0.5f;

            sens = 0;
        }
    }
}
