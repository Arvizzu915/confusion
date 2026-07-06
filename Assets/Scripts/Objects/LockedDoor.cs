using System;
using UnityEngine;

public class LockedDoor : IInspectionable
{
    [SerializeField] private GameObject door;

    public override void Inspect()
    {
        base.Inspect();

        AnalyzingMode.Instance.EnterAnalyzeMode(true);
    }

    public override void UseItem(int itemIndex, ItemSlotUI itemSlot)
    {
        if (index == itemIndex)
        {
            AnalyzingMode.Instance.ExitAnalyzeMode();
            inspectable = false;
            UnlockDoor();
            gameObject.SetActive(false);
        }
    }

    public void UnlockDoor()
    {
        door.SetActive(true);
    }
}
