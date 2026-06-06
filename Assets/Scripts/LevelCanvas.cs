using TMPro;
using UnityEngine;

public class LevelCanvas : MonoBehaviour
{
    public static LevelCanvas instance;

    public GameObject HUDPanel, inventoryPanel;
    public TextMeshProUGUI interactableText, checkingObjText;

    public CanvasGroup inputInstructionsCanvas, selectingInputsCanvas, movingInputsCanvas, inspectingInputsCanvas;
    private CanvasGroup currentInputCanvas;

    private void Awake()
    {
        instance = this;
    }

    public void OpenInventory(CanvasGroup inputsInstructions)
    {
        LeanTween.alphaCanvas(inputInstructionsCanvas, 0, 0f);
        LeanTween.alphaCanvas(inputInstructionsCanvas, 1, .15f);

        HUDPanel.SetActive(false);
        inventoryPanel.SetActive(true);

        FadeInPanel(inputsInstructions);
        currentInputCanvas = inputsInstructions;
    }

    public void CloseInventory()
    {
        ChangeToPlayingHUD();
        LeanTween.alphaCanvas(inputInstructionsCanvas, 0, .15f);
        InputManager.Instance.SwitchToGameplay();

        FadeOutPanel(currentInputCanvas);
    }

    public void ChangeToPlayingHUD()
    {
        HUDPanel.SetActive(true);
        inventoryPanel.SetActive(false);
    }

    public void FadeInPanel(CanvasGroup ui)
    {
        ui.gameObject.SetActive(true);
        ui.alpha = 0.0f;
        LeanTween.alphaCanvas(ui, 1, 0.15f);
    }

    public void FadeOutPanel(CanvasGroup ui)
    {
        LeanTween.alphaCanvas(ui, 0, 0.15f);
    }
}
