using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    public Item item;

    public Button button;

    [SerializeField] private Image itemImage;
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

        itemImage.sprite = item.icon;
    }
}