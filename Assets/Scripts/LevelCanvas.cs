using TMPro;
using UnityEngine;

public class LevelCanvas : MonoBehaviour
{
    public GameObject HUDPanel, inspectingPanel;
    public TextMeshProUGUI interactableText, checkingObjText;

    public void ChangeToInspectHUD()
    {
        HUDPanel.SetActive(false);
        inspectingPanel.SetActive(true);
    }

    public void ChangeToPlayingHUD()
    {
        HUDPanel.SetActive(true);
        inspectingPanel.SetActive(false);
    }
}
