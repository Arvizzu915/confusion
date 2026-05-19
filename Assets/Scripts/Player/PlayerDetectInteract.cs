using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDetectInteract : MonoBehaviour
{
    public static PlayerDetectInteract instance;

    [SerializeField] private InputManager inputManager;
    public Camera playerCamera;

    [SerializeField] private PlayerManager playerManager;
    private Interactuable currentInteractable = null;
    [SerializeField] private LayerMask detectLayer;
    [SerializeField] private float detectRayLength = 3.5f;

    public bool checkingObject = false, analyzing = false;
    [HideInInspector] public KeyPiece currentKeyPiece = null;
    public Transform inspectPoint;
    public GameObject inspectCamera, zoomCamera, inspectLight, lantern, bow;

    private bool zooming = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        inputManager.inputs.Playing.Interact.performed += Interact;

        inputManager.inputs.Inspecting.Take.performed += TakeInspectingObject;
        inputManager.inputs.Inspecting.Zoom.performed += Zoom;
        inputManager.inputs.Inspecting.Reset.performed += ResetInspectingObjectPos;

    }

    private void OnDisable()
    {
        inputManager.inputs.Playing.Interact.performed -= Interact;

        inputManager.inputs.Inspecting.Take.performed -= TakeInspectingObject;
        inputManager.inputs.Inspecting.Zoom.performed -= Zoom;
        inputManager.inputs.Inspecting.Reset.performed -= ResetInspectingObjectPos;
    }

    void Update()
    {
        if (analyzing) return;

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out RaycastHit hit, detectRayLength, detectLayer))
        {
            if (hit.transform.TryGetComponent(out Interactuable interactable))
            {
                Debug.DrawRay(ray.origin, ray.direction * detectRayLength, Color.green);
                currentInteractable = interactable;
                interactable.Hover();
            }
            else
            {
                Debug.DrawRay(ray.origin, ray.direction * detectRayLength, Color.red);
                currentInteractable = null;
                playerManager.PlayerUIManager.CanInteract(false, "");
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * detectRayLength, Color.red);
            currentInteractable = null;
            playerManager.PlayerUIManager.CanInteract(false, "");
        }

    }

    private void Interact(InputAction.CallbackContext ctx)
    {
        if (checkingObject) return;

        if (currentInteractable != null)
        {
            currentInteractable.Interact(playerManager);
            if (currentInteractable.inspectable)
            {
                checkingObject = true;
            }
        }
    }

    private void TakeInspectingObject(InputAction.CallbackContext ctx)
    {
        if (!checkingObject) return;

        if (ctx.performed)
        {
            checkingObject = false;
            playerManager.inputManager.SwitchToGameplay();

            currentKeyPiece.TakeObject();
            inspectLight.SetActive(false);
            inspectCamera.SetActive(false);
            zoomCamera.SetActive(false);
            lantern.SetActive(true);
        }
    }

    private void Zoom(InputAction.CallbackContext ctx)
    {
        if (!checkingObject) return;

        if (zooming)
        {
            zoomCamera.SetActive(false);
            zooming = false;
        }
        else
        {
            zoomCamera.SetActive(true);
            zooming = true;
        }
    }

    private void ResetInspectingObjectPos(InputAction.CallbackContext ctx)
    {
        if (!checkingObject) return;

        if (ctx.performed)
        {
            StartCoroutine(currentKeyPiece.ResetPosition());
        }
    }
}
