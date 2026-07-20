using UnityEngine;

public abstract class KeyObject : Interactuable
{

    public override void Interact(PlayerManager player)
    {
        TakeObject();
    }

    public virtual void TakeObject()
    {
        GetPicked();
    }

    public virtual void GetPicked()
    {
        gameObject.SetActive(false);
    }
}
