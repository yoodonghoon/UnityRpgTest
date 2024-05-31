using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : UIWindow
{
    public Button ExitButton;
    public List<InventorySlot> Slots = new ();

    void Start()
    {
        ExitButton.onClick.AddListener(ExitButtonAction);
        windowName = "Inventory";
    }

    void ExitButtonAction() => UIManager.Instance.UISet(windowName, false);

    public void AddItem(Item item)
    {
        if(item.ItemType ==ItemType.POTION && CheckItem(item.ItemIndex))
            return;
        
        foreach (var slotItem in Slots)
        {
            if (slotItem.ItemData == null)
            {
                slotItem.SetItemData(item);
                break;
            }
        }
    }

    private bool CheckItem(int index)
    {
        foreach (var slotItem in Slots)
        {
            if (slotItem != null)
                continue;

            if (slotItem.ItemData.ItemIndex == index)
            {
                slotItem.SetCount(slotItem.ItemCount + 1);
                return true;
            }
        }

        return false;
    }

    public void UseItem()
    { 
    
    }
}
