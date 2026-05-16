using UnityEngine;

public class LockSO : Interactuable
{
    [SerializeField] private int[] indexes;

    public override void Interact(PlayerManager player)
    {
        if (ObjectsInventory.instance.CheckKeyPiece(indexes))
        {

        }
    }
}
