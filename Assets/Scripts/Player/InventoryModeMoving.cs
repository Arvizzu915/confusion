using UnityEngine;

public class InventoryModeMoving : InventoryMode
{
    public override void ConfirmMode(ObjectsInventory inventory)
    {
        
    }

    public override void EnterMode(ObjectsInventory inventory)
    {
        
    }

    public override void EscapeMode(ObjectsInventory inventory)
    {
        inventory.ChangeMode(inventory.selectMode);
    }

    public override void MoveMode(ObjectsInventory inventory)
    {
        
    }
    public override void OnChangeSlot(ObjectsInventory inventory)
    {

    }
}
