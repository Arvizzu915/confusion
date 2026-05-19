using UnityEngine;

public abstract class IInspectionable : Interactuable
{
    [SerializeField] private GameObject inspectingCamera;

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
}
