using UnityEngine;

public interface IInteractuable
{
    public void Interact(PlayerManager player);

    public void Hover(PlayerUIManager UIManager);
}
