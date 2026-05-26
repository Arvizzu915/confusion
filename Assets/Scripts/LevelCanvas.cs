using TMPro;
using UnityEngine;

public class LevelCanvas : MonoBehaviour
{
    public static LevelCanvas instance;

    public GameObject HUDPanel, inspectingPanel;
    public TextMeshProUGUI interactableText, checkingObjText;

    private void Awake()
    {
        instance = this;
    }

    public void OpenInventoryHUD()
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
