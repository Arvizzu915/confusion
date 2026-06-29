using UnityEngine;

public abstract class IInspectionable : Interactuable
{
    [SerializeField] private GameObject inspectingCamera;

    public int index;

    public override void Interact(PlayerManager player)
    {
        Inspect();
    }

    public virtual void Inspect()
    {
        AnalyzingMode.Instance.currentObj = this;
        inspectingCamera.SetActive(true);
    }

    public virtual void StopInspecting()
    {
        inspectingCamera.SetActive(false);
    }

    public virtual void UseItem(int itemIndex, ItemSlotUI itemSlot)
    {
        if (index == itemIndex)
        {
            gameObject.SetActive(false);
            AnalyzingMode.Instance.ExitAnalyzeMode();
        }
    }
}
