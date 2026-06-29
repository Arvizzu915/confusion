using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class AnalyzingMode : MonoBehaviour
{
    public static AnalyzingMode Instance;

    public IInspectionable currentObj;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InputManager.Instance.inputs.Analyze.Return.performed += ExitAnalyze;
        InputManager.Instance.inputs.Analyze.Use.performed += SubmitSelectedSlot;

        
    }

    private void OnDisable()
    {
        InputManager.Instance.inputs.Analyze.Return.performed -= ExitAnalyze;
        InputManager.Instance.inputs.Analyze.Use.performed -= SubmitSelectedSlot;
    }

    public void EnterAnalyzeMode()
    {
        Flashlight.instance.ChangeToInspectingLight(false);

        ObjectsInventory.instance.OpenMenu(null);

        InputManager.Instance.SwitchToAnalyze();
        PlayerManager.instance.PlayerUIManager.CanInteract(false, "");
        PlayerDetectInteract.instance.analyzing = true;
        PlayerDetectInteract.instance.bow.SetActive(false);
    }

    private void ExitAnalyze(InputAction.CallbackContext context)
    {
        ExitAnalyzeMode();
    }

    public void ExitAnalyzeMode()
    {
        ObjectsInventory.instance.CloseMenu();
        Flashlight.instance.ChangeToInspectingLight(true);

        PlayerDetectInteract.instance.checkingObject = false;
        InputManager.Instance.SwitchToGameplay();
        PlayerDetectInteract.instance.analyzing = false;
        LevelCanvas.instance.ChangeToPlayingHUD();
        PlayerDetectInteract.instance.bow.SetActive(true);

        currentObj.StopInspecting();
        currentObj = null;
    }

    

    private void SubmitSelectedSlot(InputAction.CallbackContext ctx)
    {
        if (!ObjectsInventory.instance.menuOpen) return;

        GameObject selectedObject = EventSystem.current.currentSelectedGameObject;

        for (int i = 0; i < ObjectsInventory.instance.slotButtons.Length; i++)
        {
            if (ObjectsInventory.instance.slotButtons[i].gameObject == selectedObject)
            {
                UseSlot(i);
                return;
            }
        }
    }

    public void UseSlot(int slotIndex)
    {
        Item item = ObjectsInventory.instance.slotButtons[slotIndex].item;

        if (item == null) return;

        currentObj.UseItem(item.index, ObjectsInventory.instance.currentSlotButton);


        //Debug.Log("Using item: " + item.itemName);
    }
}
