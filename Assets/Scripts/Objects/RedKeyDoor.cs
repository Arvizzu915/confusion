using System;
using UnityEngine;

public class RedKeyDoor : IInspectionable
{
    [SerializeField] private GameObject doorGO, doorHandle;
    [SerializeField] private Collider coll;

    public override void Inspect()
    {
        base.Inspect();

        AnalyzingMode.Instance.EnterAnalyzeMode();
    }

    public override void UseItem(int itemIndex, ItemSlotUI itemSlot)
    {
        if (index == itemIndex)
        {
            itemSlot.RemoveItem();
            AnalyzingMode.Instance.ExitAnalyzeMode();
            doorHandle.SetActive(true);
            inspectable = false;
            coll.enabled = false;
        }
    }

    public void UnlockDoor()
    {
        doorGO.SetActive(true);
        gameObject.SetActive(false);
    }
}
