using Unity.VisualScripting;
using UnityEngine;

public abstract class Interactuable : MonoBehaviour
{
    [SerializeField] private string interactText = "Interact";
    public bool canInteract = true;

    public abstract void Interact(PlayerManager player);


    public virtual void Hover()
    {
        PlayerManager.instance.PlayerUIManager.CanInteract(canInteract, interactText);
    }
}
