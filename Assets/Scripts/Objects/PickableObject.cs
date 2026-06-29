using System;
using System.Collections;
using UnityEngine;

public class PickableObject : Interactuable
{
    public Item item;

    public PlayerDetectInteract interactScript = null;

    public override void Interact(PlayerManager player)
    {
        TryTakeObject();
    }

    public virtual void TryTakeObject()
    {
        ObjectsInventory.instance.TryPickObject(this);
    }

    public virtual void CancelAdd()
    {

    }

    public virtual void GetPicked()
    {
        Flashlight.instance.ChangeToInspectingLight(true);
        gameObject.SetActive(false);
    }
}
