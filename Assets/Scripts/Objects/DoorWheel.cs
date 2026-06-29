using UnityEngine;

public class DoorWheel : Interactuable
{
    [SerializeField] private RedKeyDoor door;

    public override void Interact(PlayerManager player)
    {
        door.UnlockDoor();
        gameObject.SetActive(false);
    }
}
