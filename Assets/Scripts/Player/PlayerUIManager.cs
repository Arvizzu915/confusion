using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    public LevelCanvas levelCanvas;

    private void Start()
    {
        levelCanvas = PlayerManager.instance.PlayerUIManager.levelCanvas;
    }

    public void CanInteract(bool isInteractable, string interactMode)
    {
       if (levelCanvas == null) return;

       levelCanvas.interactableText.gameObject.SetActive(isInteractable);
       levelCanvas.interactableText.text = "[F] to " + interactMode;
    }

    public void ActivateCheckingObjectText(bool checking)
    {
        levelCanvas.checkingObjText.gameObject.SetActive(checking);
        
    }
}
