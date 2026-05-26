using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyPiece : Interactuable
{
    [SerializeField] private int index = 0;
    [SerializeField] private Collider coll;

    [SerializeField] private Item item;

    private PlayerDetectInteract interactScript = null;

    [Header("Inspect")]
    [SerializeField] private float inspectDuration = 0.5f;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 150f;

    private bool canRotate = false;
    private InputAction lookAction;
    private Quaternion inspectPointRot;

    [Header("Inventory")]
    [SerializeField] private GameObject inventoryPanel;

    private void Update()
    {
        if (!canRotate) return;

        RotateObject();
    }

    public override void Interact(PlayerManager player)
    {
        CheckObject();
    }

    private void CheckObject()
    {
        PlayerManager.instance.inputManager.SwitchToInspect();

        PlayerDetectInteract.instance.inspectCamera.SetActive(true);

        StartCoroutine(MoveToInspectPoint(PlayerDetectInteract.instance.inspectPoint));
        StartCoroutine(TurnOnInspectLight());

        coll.enabled = false;

        interactScript = PlayerDetectInteract.instance;

        interactScript.currentKeyPiece = this;

        interactScript.lantern.SetActive(false);

        Time.timeScale = 0;

        PlayerManager.instance.PlayerUIManager.ActivateCheckingObjectText(true);
        PlayerManager.instance.PlayerUIManager.CanInteract(false, "");

        lookAction = PlayerManager.instance.GetComponent<InputManager>().inputs.Inspecting.Rotate;
    }

    public void TryTakeObject()
    {
        Debug.Log("open");

        ObjectsInventory.instance.OpenMenu();
    }

    private void AddToInventory()
    {
        PlayerManager.instance.PlayerUIManager.ActivateCheckingObjectText(false);

        interactScript = PlayerDetectInteract.instance;

        interactScript.currentKeyPiece = null;

        Time.timeScale = 1;
        ObjectsInventory.instance.AddKeyPiece(index);

        gameObject.SetActive(false);
    }

    private void RotateObject()
    {
        Vector2 lookInput = lookAction.ReadValue<Vector2>();

        float rotX = lookInput.y * rotationSpeed * Time.unscaledDeltaTime;
        float rotY = -lookInput.x * rotationSpeed * Time.unscaledDeltaTime;

        Transform cam = PlayerDetectInteract.instance.playerCamera.transform;

        // Horizontal rotation around camera up axis
        transform.Rotate(
            cam.up,
            rotY,
            Space.World
        );

        // Vertical rotation around camera right axis
        transform.Rotate(
            cam.right,
            rotX,
            Space.World
        );
    }

    private IEnumerator MoveToInspectPoint(Transform inspectPoint)
    {
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        Vector3 targetPos = inspectPoint.position;
        inspectPointRot = inspectPoint.rotation;

        float timer = 0f;

        while (timer < inspectDuration)
        {
            timer += Time.unscaledDeltaTime;

            float t = timer / inspectDuration;

            t = Mathf.SmoothStep(0f, 1f, t);

            
            transform.SetPositionAndRotation(Vector3.Lerp(
                startPos,
                targetPos,
                t
            ), Quaternion.Slerp(
                startRot,
                inspectPointRot,
                t
            ));

            yield return null;
        }

        transform.SetPositionAndRotation(targetPos, inspectPointRot);

        canRotate = true;
    }

    public IEnumerator ResetPosition()
    {
        Quaternion startRot = transform.rotation;

        float timer = 0f;

        while (timer < inspectDuration)
        {
            timer += Time.unscaledDeltaTime;

            float t = timer / inspectDuration;

            t = Mathf.SmoothStep(0f, 1f, t);

            transform.rotation = Quaternion.Slerp(startRot, inspectPointRot, t);

            yield return null;
        }

        transform.rotation = inspectPointRot;
    }

    private IEnumerator TurnOnInspectLight()
    {
        float timer = 0f;

        while (timer < inspectDuration + 0.03f)
        {
            timer += Time.unscaledDeltaTime;

            float t = timer / inspectDuration;

            _ = Mathf.SmoothStep(0f, 1f, t);

            yield return null;
        }

        PlayerDetectInteract.instance.inspectLight.SetActive(true);
    }
    
}
