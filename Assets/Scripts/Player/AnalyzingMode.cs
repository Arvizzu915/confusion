using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class AnalyzingMode : MonoBehaviour
{
    public static AnalyzingMode Instance;

    public IInspectionable currentObj;
    private InputAction move;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InputManager.Instance.inputs.Analyze.Return.performed += ExitAnalyze;
        InputManager.Instance.inputs.Analyze.Use.performed += SubmitSelectedSlot;
        InputManager.Instance.inputs.Analyze.Move.performed += MoveInputs;
        InputManager.Instance.inputs.Analyze.Move.canceled += CancelInput;

        move = InputManager.Instance.inputs.Analyze.Move;
    }

    private void Update()
    {
        if (currentObj == null) return;

        currentObj.MoveDirection(move.ReadValue<Vector2>());
    }

    private void OnDisable()
    {
        InputManager.Instance.inputs.Analyze.Return.performed -= ExitAnalyze;
        InputManager.Instance.inputs.Analyze.Use.performed -= SubmitSelectedSlot;
        InputManager.Instance.inputs.Analyze.Move.performed -= MoveInputs;
        InputManager.Instance.inputs.Analyze.Move.canceled -= CancelInput;
    }

    public void EnterAnalyzeMode(bool openInventory)
    {
        Flashlight.instance.ChangeToInspectingLight(false);

        if (openInventory)
        {
            ObjectsInventory.instance.OpenMenu(null);
        }
        
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
        currentObj.Use();

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

    private void MoveInputs(InputAction.CallbackContext ctx)
    {
        currentObj.MoveInputs(ctx.ReadValue<Vector2>());
    }

    private void CancelInput(InputAction.CallbackContext ctx)
    {
        currentObj.CancelInput();
    }
}
