using UnityEngine;

public abstract class InventoryMode
{
    public abstract void EnterMode(ObjectsInventory inventory);
    public abstract void ConfirmMode(ObjectsInventory inventory);
    public abstract void EscapeMode(ObjectsInventory inventory);

    public abstract void MoveMode(ObjectsInventory inventory);

    public abstract void OnChangeSlot(ObjectsInventory inventory);

}
