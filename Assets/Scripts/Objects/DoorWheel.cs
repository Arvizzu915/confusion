using UnityEngine;

public class DoorWheel : Interactuable
{
    [SerializeField] private RedKeyDoor door;

    public override void Interact(PlayerManager player)
    {
        if (door.locked)
        {
            door.locked = false;
        }
        else
        {
            door.locked = true;
        }
    }
}
