using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class AnalyzingMode : MonoBehaviour
{
    public static AnalyzingMode Instance;

    public IInspectionable currentObj;

    [Header("References")]
    [SerializeField] private ItemSlotUI[] slotButtons;

    private int selectedIndex = 0;
    private bool menuOpen = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InputManager.Instance.inputs.Analyze.Return.performed += ExitAnalyze;
        InputManager.Instance.inputs.Analyze.Use.performed += SubmitSelectedSlot;

        for (int i = 0; i < slotButtons.Length; i++)
        {
            int index = i;

            slotButtons[i].button.onClick.AddListener(() =>
            {
                SelectSlot(index);
                UseSlot(index);
            });

            
            if (!slotButtons[i].gameObject.TryGetComponent<EventTrigger>(out var trigger))
                trigger = slotButtons[i].gameObject.AddComponent<EventTrigger>();

            EventTrigger.Entry pointerEnter = new()
            {
                eventID = EventTriggerType.PointerEnter
            };

            pointerEnter.callback.AddListener(_ =>
            {
                SelectSlot(index);
            });

            trigger.triggers.Add(pointerEnter);
        }
    }

    private void OnDisable()
    {
        InputManager.Instance.inputs.Analyze.Return.performed -= ExitAnalyze;
        InputManager.Instance.inputs.Analyze.Use.performed -= SubmitSelectedSlot;
    }

    public void EnterAnalyzeMode()
    {
        OpenMenu();

        InputManager.Instance.SwitchToAnalyze();
        PlayerManager.instance.PlayerUIManager.CanInteract(false, "");
        PlayerDetectInteract.instance.analyzing = true;
        PlayerDetectInteract.instance.lantern.GetComponent<Light>().intensity = 3;
        LevelCanvas.instance.ChangeToInspectHUD();
        PlayerDetectInteract.instance.bow.SetActive(false);
        Time.timeScale = 0.0f;
    }

    private void ExitAnalyze(InputAction.CallbackContext context)
    {
        ExitAnalyzeMode();
    }

    public void ExitAnalyzeMode()
    {
        CloseMenu();

        PlayerDetectInteract.instance.checkingObject = false;
        InputManager.Instance.SwitchToGameplay();
        PlayerDetectInteract.instance.lantern.GetComponent<Light>().intensity = 52;
        PlayerDetectInteract.instance.analyzing = false;
        LevelCanvas.instance.ChangeToPlayingHUD();
        PlayerDetectInteract.instance.bow.SetActive(true);
        Time.timeScale = 1.0f;

        currentObj.StopInspecting();
        currentObj = null;
    }

    public void OpenMenu()
    {
        menuOpen = true;

        selectedIndex = 0;
        SelectSlot(selectedIndex);

        Time.timeScale = 0f;
    }

    public void CloseMenu()
    {
        if (!menuOpen) return;

        menuOpen = false;

        Time.timeScale = 1f;
    }

    private void SubmitSelectedSlot(InputAction.CallbackContext ctx)
    {
        if (!menuOpen) return;

        GameObject selectedObject = EventSystem.current.currentSelectedGameObject;

        for (int i = 0; i < slotButtons.Length; i++)
        {
            if (slotButtons[i].gameObject == selectedObject)
            {
                UseSlot(i);
                return;
            }
        }
    }

    private void SelectSlot(int index)
    {
        if (index < 0 || index >= slotButtons.Length) return;

        selectedIndex = index;

        EventSystem.current.SetSelectedGameObject(slotButtons[selectedIndex].gameObject);
    }

    private void UseSlot(int slotIndex)
    {
        Item item = slotButtons[slotIndex].item;

        if (item == null)
            return;

        Debug.Log("Using item: " + item.itemName);

        currentObj.UseItem(item.index);
    }
}
