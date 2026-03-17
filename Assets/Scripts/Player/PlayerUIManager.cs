using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;

    private GameObject canvas;
    private LevelCanvas levelCanvas;

    public void SetManager(PlayerManager playerManagerRef)
    {
        playerManager = playerManagerRef;

        canvas = playerManager.canvas;
        levelCanvas = playerManager.levelCanvas;
    }

    public void CanInteract(bool isInteractable, string interactMode)
    {
        if (levelCanvas == null) return;

       levelCanvas.interactableText.gameObject.SetActive(isInteractable);
       levelCanvas.interactableText.text = "\"E\" to " + interactMode;
    }
}
