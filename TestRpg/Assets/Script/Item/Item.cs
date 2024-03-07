using UnityEngine;
using UnityEngine.UI;

public enum ItemType
{
    EQUIP,
    POTION
}

public class Item
{
    public int ItemIndex = 0;
    public ItemType ItemType;
    public Sprite ItemSprite { get; set; }
}