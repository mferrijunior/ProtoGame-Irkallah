using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = null, menuName = "Scriptable Objects/Item", order = 0)]
public class Item : ScriptableObject
{
    public Sprite itemSprite;
    public string itemDescription;
    public bool isKey;
}
