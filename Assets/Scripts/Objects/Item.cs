using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Item", menuName = "Objects/item")]
public class Item : ScriptableObject
{
    public int index;

    public string itemName, description;

    public Sprite icon;

    public bool cumulative = false;
    public int quantity = 3;
}