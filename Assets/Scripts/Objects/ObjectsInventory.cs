using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectsInventory : MonoBehaviour
{
    public static ObjectsInventory instance;

    public bool[] keyPieces;

    [Header("References")]
    public ItemSlotUI[] slotButtons;

    public int selectedIndex = 0;
    public bool menuOpen = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
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

    public void AddKeyPiece(int pieceIndex)
    {
        keyPieces[pieceIndex] = true;
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

    public void OpenMenu()
    {
        menuOpen = true;

        LevelCanvas.instance.OpenInventoryHUD();

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

    public void AddItemToInventory(Item newItem)
    {
        if (newItem == null) return;
    }
}
