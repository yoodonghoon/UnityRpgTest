using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManger : SingletonCommon<InventoryManger>
{
    public Inventory Inventory { get; set; }

    public void AddItem(int itemIndex)
    {
        Item newitem = new ();
        newitem.ItemIndex = itemIndex;

        var tableData = ItemTable.Instance.GetData(newitem.ItemIndex);

        newitem.ItemType = (ItemType)tableData.Type;
        newitem.ItemSprite = Resources.Load<Sprite>(tableData.ImagePath);

        Inventory.AddItem(newitem);
    }
}
