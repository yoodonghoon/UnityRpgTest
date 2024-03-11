using System.Collections.Generic;
using UnityEngine;

public class InventoryManger : SingletonCommon<InventoryManger>
{
    public Inventory Inventory { get; set; }
    public List<Item> OpenBeforeItem = new();

    public void AddItem(int itemIndex)
    {
        Item newitem = new ();
        newitem.ItemIndex = itemIndex;

        var tableData = ItemTable.Instance.GetData(newitem.ItemIndex);

        newitem.ItemType = (ItemType)tableData.Type;
        newitem.ItemSprite = Resources.Load<Sprite>(tableData.ImagePath);

        if (Inventory != null)
        {
            if(OpenBeforeItem.Count != 0)
            {
                for(int i = 0; i < OpenBeforeItem.Count; ++i)
                {
                    Inventory.AddItem(OpenBeforeItem[i]);
                }
            }
            Inventory.AddItem(newitem);
        }
        else
            OpenBeforeItem.Add(newitem);
    }

    public void InventoryInit()
    {
        if (OpenBeforeItem.Count != 0)
        {
            for (int i = 0; i < OpenBeforeItem.Count; ++i)
            {
                Inventory.AddItem(OpenBeforeItem[i]);
            }
        }

        OpenBeforeItem.Clear();
    }
}