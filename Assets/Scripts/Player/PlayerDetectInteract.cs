using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDetectInteract : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;

    [SerializeField] private PlayerManager playerManager;
    private IInteractuable currentInteractable = null;
    [SerializeField] private LayerMask detectLayer;

    private void OnEnable()
    {
        inputManager.inputs.Playing.Interact.performed += Interact;
    }

    private void OnDisable()
    {
        inputManager.inputs.Playing.Interact.performed -= Interact;
    }

    void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 2f, detectLayer))
        {
            if (hit.transform.TryGetComponent(out IInteractuable interactable))
            {
                Debug.DrawRay(transform.position, transform.forward * 2, Color.green);
                currentInteractable = interactable;
                interactable.Hover(playerManager.PlayerUIManager);
            }
            else
            {
                Debug.DrawRay(transform.position, transform.forward * 2, Color.red);
                currentInteractable = null;
                playerManager.PlayerUIManager.CanInteract(false, "");
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * 2, Color.red);
            currentInteractable = null;
            playerManager.PlayerUIManager.CanInteract(false, "");
        }

    }

    private void Interact(InputAction.CallbackContext ctx)
    {
        currentInteractable?.Interact(playerManager);
    }
}
