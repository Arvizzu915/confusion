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
    private float targetHeigth = 1.5f;

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

    private void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, targetHeigth, transform.localPosition.z), Time.deltaTime * 10f);

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

    private void ChangeSensibility(InputAction.CallbackContext ctx)
    {
        if (sens == 0)
        {
            sensibilityX = 310;
            sensibilityY = 310;

            sens = 1;
        }
        else if (sens == 1)
        {
            sensibilityX = 50;
            sensibilityY = 50;

            sens = 0;
        }
    }
}
