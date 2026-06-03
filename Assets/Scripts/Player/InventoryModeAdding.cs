using UnityEngine;

public class InventoryModeAdding : InventoryMode
{
    public override void ConfirmMode(ObjectsInventory inventory)
    {

        TryAddItemToInventory(inventory, inventory.currentItem);
    }

    public override void EnterMode(ObjectsInventory inventory)
    {

    }

    public override void EscapeMode(ObjectsInventory inventory)
    {
        inventory.CloseMenu();
        inventory.currentPickable.CancelAdd();
    }

    public override void MoveMode(ObjectsInventory inventory)
    {
        throw new System.NotImplementedException();
    }

    public void TryAddItemToInventory(ObjectsInventory inventory, Item newItem)
    {
        if (newItem == null) return;

        if (inventory.slotButtons[inventory.selectedIndex].occupied && inventory.slotButtons[inventory.selectedIndex].item.index != newItem.index)
        {
            Debug.Log("can't do sowwy");
        }
        else
        {
            inventory.AddItemToInventory(newItem);
        }
    }
}
