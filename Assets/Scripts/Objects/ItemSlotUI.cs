using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    public Item item;

    public Button button;

    [SerializeField] private Image itemImage;

    public bool occupied = false;

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

    public void SetImage()
    {
        if (item == null) return;

        Color c = itemImage.color;
        c.a = 1f;
        itemImage.color = c;

        itemImage.sprite = item.icon;
        itemImage.color = Color.white;
    }

    public void RemoveItem()
    {
        item = null;
        occupied = false;

        Color c = itemImage.color;
        c.a = 0f;
        itemImage.color = c;
    }

    public void AddItemToButton(Item newItem)
    {
        item = newItem;
        occupied = true;

        Color c = itemImage.color;
        c.a = 1f;
        itemImage.color = c;

        itemImage.sprite = item.icon;
        itemImage.color = Color.white;
    }
}