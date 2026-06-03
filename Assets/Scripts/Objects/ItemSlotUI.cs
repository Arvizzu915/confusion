using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    public Item item;

    public Button button;

    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemCounterText;

    public bool occupied = false;
    public int currentItems = 0;

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
        currentItems = 0;
        itemCounterText.gameObject.SetActive(false);

        item = null;
        occupied = false;

        Color c = itemImage.color;
        c.a = 0f;
        itemImage.color = c;
    }

    public void RemoveCumulative()
    {
        currentItems--;

        if (currentItems <= 0)
        {
            RemoveItem();
        }
    }

    public void AddItemToButton(Item newItem)
    {
        item = newItem;
        occupied = true;

        SetImage();
    }

    public void AddCumulative(Item newItem, int quantity)
    {
        if (currentItems <= 0)
        {
            item = newItem;
            occupied = true;

            itemCounterText.gameObject.SetActive(true);

            SetImage();
        }

        currentItems += quantity;
        itemCounterText.text = currentItems.ToString();
    }
}