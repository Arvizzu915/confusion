using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDetectInteract : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;

    [SerializeField] private PlayerManager playerManager;
    private Interactuable currentInteractable = null;
    [SerializeField] private LayerMask detectLayer;
    [SerializeField] private float detectRayLength = 3.5f;

    private void Start()
    {
        inputManager.inputs.Playing.Interact.performed += Interact;
    }

    private void OnDisable()
    {
        inputManager.inputs.Playing.Interact.performed -= Interact;
    }

    void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, detectRayLength))
        {

            if (hit.transform.TryGetComponent(out Interactuable interactable))
            {
                Debug.DrawRay(transform.position, transform.forward * detectRayLength, Color.green);
                currentInteractable = interactable;
                interactable.Hover();
            }
            else
            {
                Debug.DrawRay(transform.position, transform.forward * detectRayLength, Color.red);
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
        if (currentInteractable != null)
        {
            currentInteractable.Interact(playerManager);
        }
    }
}
