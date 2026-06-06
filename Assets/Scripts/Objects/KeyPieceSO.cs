using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyPiece : PickableObject
{
    [SerializeField] private Collider coll;

    [Header("Inspect")]
    [SerializeField] private float inspectDuration = 0.5f;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 150f;

    private bool canRotate = false;
    private InputAction lookAction;
    private Quaternion inspectPointRot;

    [Header("Inventory")]
    [SerializeField] private GameObject inventoryPanel;

    [SerializeField] private Vector3 startingPos;
    [SerializeField] private Quaternion startingRot;

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

        PlayerDetectInteract.instance.checkingObject = true;
        PlayerManager.instance.inputManager.SwitchToInspect();

        PlayerDetectInteract.instance.inspectCamera.SetActive(true);

        StartCoroutine(MoveToInspectPoint(PlayerDetectInteract.instance.inspectPoint));
        //StartCoroutine(TurnOnInspectLight());

        coll.enabled = false;

        interactScript = PlayerDetectInteract.instance;

        interactScript.currentKeyPiece = this;

        //interactScript.flashlight.SetActive(false);

        PlayerManager.instance.PlayerUIManager.ActivateCheckingObjectText(true);
        PlayerManager.instance.PlayerUIManager.CanInteract(false, "");

        lookAction = PlayerManager.instance.GetComponent<InputManager>().inputs.Inspecting.Rotate;
    }

    public override void TryTakeObject()
    {
        base.TryTakeObject();

        PlayerDetectInteract.instance.checkingObject = false;
        PlayerDetectInteract.instance.zoomCamera.SetActive(false);
        canRotate = false;
        StartCoroutine(ResetPosition());
        PlayerManager.instance.PlayerUIManager.ActivateCheckingObjectText(false);
    }

    public override void CancelAdd()
    {
        base.CancelAdd();

        PlayerDetectInteract.instance.checkingObject = false;
        //interactScript.flashlight.SetActive(true);
        PlayerDetectInteract.instance.inspectCamera.SetActive(false);

        StartCoroutine(MoveToStartPoint());
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

    private IEnumerator MoveToStartPoint()
    {
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        Vector3 targetPos = startingPos;
        Quaternion targetRot = startingRot;

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
                targetRot,
                t
            ));

            yield return null;
        }

        transform.SetPositionAndRotation(targetPos, targetRot);

        canRotate = true;
        coll.enabled = true;
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
