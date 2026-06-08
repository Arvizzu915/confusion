using UnityEngine;

public class InventoryModeSelecting : InventoryMode
{
    public override void ConfirmMode(ObjectsInventory inventory)
    {
        
    }

    public override void EnterMode(ObjectsInventory inventory)
    {
        
    }

    public override void EscapeMode(ObjectsInventory inventory)
    {
        inventory.CloseMenu();
    }

    public override void MoveMode(ObjectsInventory inventory)
    {
        inventory.ChangeMode(inventory.moveMode);
    }

    public override void OnChangeSlot(ObjectsInventory inventory)
    {
        inventory.UpdateItemInfo();
    }
}
