using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ObjectsInventory : MonoBehaviour
{
    public static ObjectsInventory instance;

    public bool[] keyPieces;

    [Header("References")]
    public ItemSlotUI[] slotButtons;

    public int selectedIndex = 0;
    public bool menuOpen = false;

    public Item currentItem = null;
    public PickableObject currentPickable;

    public bool itemSelected = false;

    InventoryMode currentMode = null;
    readonly public InventoryMode selectMode = new InventoryModeSelecting();
    readonly public InventoryMode moveMode = new InventoryModeMoving();
    readonly public InventoryMode examinateMode = new InventoryModeExaminating();
    readonly public InventoryMode addMode = new InventoryModeAdding();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        currentMode = selectMode;

        InputManager.Instance.inputs.Inventory.Confirm.performed += Confirm;
        InputManager.Instance.inputs.Inventory.Escape.performed += Escape;
        InputManager.Instance.inputs.Inventory.Move.performed += Move;

        for (int i = 0; i < slotButtons.Length; i++)
        {
            int index = i;

            slotButtons[i].button.onClick.AddListener(() =>
            {
                SelectSlot(index);
                AnalyzingMode.Instance.UseSlot(index);
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

            slotButtons[i].SetImage();
        }
    }

    public bool CheckKeyPiece(int[] indexes)
    {
        foreach (int index in indexes)
        {
            if (!keyPieces[index])
            {
                return false;
            }
        }

        return true;
    }

    public void SelectSlot(int index)
    {
        if (index < 0 || index >= slotButtons.Length) return;

        selectedIndex = index;

        EventSystem.current.SetSelectedGameObject(slotButtons[selectedIndex].gameObject);
    }

    public void OpenInventory()
    {
        ChangeMode(selectMode);
        OpenMenu(null);
    }

    public void OpenMenu(Item selectedItem)
    {
        InputManager.Instance.SwitchToInventory();

        if (selectedItem != null)
        {
            SelectObject(selectedItem);
        }

        menuOpen = true;

        LevelCanvas.instance.OpenInventoryHUD();

        selectedIndex = 0;
        SelectSlot(selectedIndex);

        Time.timeScale = 0f;
    }

    public void CloseMenu()
    {
        if (!menuOpen) return;

        LevelCanvas.instance.ChangeToPlayingHUD();

        InputManager.Instance.SwitchToGameplay();

        menuOpen = false;

        Time.timeScale = 1f;
    }

    public void SelectObject(Item newItem)
    {
        if (newItem == null) return;

        currentItem = newItem;
        itemSelected = true;
    }

    public void ChangeMode(InventoryMode newMode)
    {
        currentMode = newMode;
        currentMode.EnterMode(this);
    }

    public void TryPickObject(PickableObject pickable)
    {
        currentPickable = pickable;
        OpenMenu(pickable.item);
        ChangeMode(addMode);
    }

    public void AddItemToInventory(Item newItem)
    {
        PlayerDetectInteract.instance.inspectCamera.SetActive(false);
        slotButtons[selectedIndex].AddItemToButton(newItem);
        currentPickable.gameObject.SetActive(false);
        ChangeMode(selectMode);
        CloseMenu();
    }

    private void SyncSelectedIndexWithEventSystem()
    {
        GameObject selectedObject = EventSystem.current.currentSelectedGameObject;

        for (int i = 0; i < slotButtons.Length; i++)
        {
            if (slotButtons[i].gameObject == selectedObject)
            {
                selectedIndex = i;
                return;
            }
        }
    }

    public void Confirm(InputAction.CallbackContext ctx)
    {
        SyncSelectedIndexWithEventSystem();
        currentMode.ConfirmMode(this);
    }

    public void Escape(InputAction.CallbackContext ctx)
    {
        currentMode.EscapeMode(this);
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        currentMode.MoveMode(this);
    }
}
