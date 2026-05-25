using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    public Item item;

    public Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public int GetItemIndex()
    {
        if (item == null)
            return -1;

        return item.index;
    }
}