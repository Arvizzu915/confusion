using UnityEngine;

public abstract class IInspectionable : Interactuable
{
    [SerializeField] private GameObject inspectingCamera;

    [SerializeField] private int index;

    public override void Interact(PlayerManager player)
    {
        Inspect();
    }

    public virtual void Inspect()
    {
        AnalyzingMode.Instance.currentObj = this;
        AnalyzingMode.Instance.EnterAnalyzeMode();

        inspectingCamera.SetActive(true);
    }

    public virtual void StopInspecting()
    {
        inspectingCamera.SetActive(false);
    }

    public virtual void UseItem(int itemIndex)
    {
        if (index == itemIndex)
        {
            gameObject.SetActive(false);
            AnalyzingMode.Instance.ExitAnalyzeMode();
        }
    }
}
