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
        LevelCanvas.instance.ChangeToInspectHUD();
        PlayerDetectInteract.instance.bow.SetActive(false);
        Time.timeScale = 0.0f;
        inspectingCamera.SetActive(true);
    }

    public virtual void StopInspecting()
    {
        LevelCanvas.instance.ChangeToPlayingHUD();
        PlayerDetectInteract.instance.bow.SetActive(true);
        Time.timeScale = 1.0f;
        inspectingCamera.SetActive(false);
    }
}
