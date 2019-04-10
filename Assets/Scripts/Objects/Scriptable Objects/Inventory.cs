using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = null, menuName = "Scriptable Objects/Inventory", order = 0)]
public class Inventory : ScriptableObject
{
    public Item currentItem;
    public List<Item> items = new List<Item>();
    public int numberOfKeys;

    public void AddItem(Item item)
    {
        // Is the item a key?
        if (item.isKey)
        {
            numberOfKeys++;
        }
        else
        {
            if (!items.Contains(item))
            {
                items.Add(item);
            }
        }
    }
}
