using TMPro;
using UnityEngine;

public class LevelCanvas : MonoBehaviour
{
    public static LevelCanvas instance;

    public GameObject HUDPanel, inventoryPanel;
    public TextMeshProUGUI interactableText, checkingObjText;

    private void Awake()
    {
        instance = this;
    }

    public void OpenInventoryHUD()
    {
        HUDPanel.SetActive(false);
        inventoryPanel.SetActive(true);
    }

    public void ChangeToPlayingHUD()
    {
        HUDPanel.SetActive(true);
        inventoryPanel.SetActive(false);
    }
}
