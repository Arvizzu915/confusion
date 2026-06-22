using System;
using UnityEngine;

public class RedKeyDoor : IInspectionable
{
    [SerializeField] private Door doorScript;

    public bool locked = true;

    public override void Inspect()
    {
        base.Inspect();

        AnalyzingMode.Instance.EnterAnalyzeMode();
    }

    public override void UseItem(int itemIndex)
    {
        if (index == itemIndex)
        {
            AnalyzingMode.Instance.ExitAnalyzeMode();
        }
    }
}
